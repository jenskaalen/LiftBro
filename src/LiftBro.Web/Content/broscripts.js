// Main configuration file. Sets up AngularJS module and routes and any other config objects

var appRoot = angular.module('main',[ 'ngAnimate', 'ngMaterial',  'ui.sortable']);
angular.module('main').directive('userExerciseDirective', function() {
    return {
        restrict: 'E',
        scope: {
            exercise: '='
},
        link: function(scope, element, attrs) {
            console.log('directive up and running');
            console.log('exercise ');
        }
    };
});


angular.module('main').factory('programUpdater', function($http) {
    return {
        saveProgram: function (program) {
            return $http.put('/api/Program/Update', program);
        }
    };
});

angular.module('main').factory('workoutExercise', function($http) {
    return {
        logWorkout: function(workoutExercise, message) {
            return $http.post('/api/WorkoutExercise/Log', { workoutExercise, message: "\"" + message + "\""});
        }
    }
});
angular.module('main').controller('newUserController', function ($scope, $http) {
    $scope.selectProgram = function () {
        $http.post('/api/Program/SelectProgram', $scope.selectedProgram)
            .success(function () {
                $scope.selectedProgram = null;
                $scope.programSet = true;
            });
    };

    $scope.loadPrograms = function () {
        $http.get('/api/Program/GetAll').success(function (programs) {
            $scope.allPrograms = programs;
        });
    };


    $scope.createNewProgram = function () {
        $scope.creatingNew = true;
        $scope.newProgram = {
            Name: "Unnamed program",
        };
    };

    $scope.create = function () {
        console.log('creating stuff');

        $http.post('/api/Program/CreateUpdateProgram', $scope.newProgram).success(function () {
            $scope.creatingNew = false;
            $scope.loadPrograms();
        });
    };

    $scope.programSet = false;
    $scope.loadPrograms();
});


angular.module('main').controller('profileController', function($scope, $http, $mdDialog, workoutExercise) {
    $scope.lastSetWorkout = 0;
    var currentWorkoutDay = 0;
    var currentExercise = 0;
    $scope.currentProgram = {};

    $scope.getNextWorkout = function () {
        $http.get('/api/WorkoutDay/GetNextWorkoutDay').success(function (workoutDay) {
            $scope.currentWorkout = workoutDay;
            $scope.currentExercise = workoutDay.exercises[0];
            $scope.originalWorkoutDay = workoutDay;
        });
    };

    $scope.$watch('currentWorkout', function() {
        $scope.currentExercise = $scope.currentWorkout.exercises[0];
    });

    $scope.getCurrentProgram = function() {
        $http.get('/api/Program/GetCurrentProgram').success(function (program) {
            $scope.currentProgram = program;
            console.log(program);
        });
    };

    $scope.updateExerciseWeight = function(exercise, weight) {

    };

    $scope.getNextWorkout();
    $scope.getCurrentProgram();

    $scope.finishExercise = function () {

        if ($scope.currentExercise.comment !== "" && $scope.currentExercise.comment != null)
        workoutExercise.logWorkout($scope.currentExercise, $scope.currentExercise.comment).success(function(id) {

        });

        var index = $scope.currentWorkout.exercises.indexOf($scope.currentExercise);
        var nextWorkoutIndex = index + 1;

        if (nextWorkoutIndex >= $scope.currentWorkout.exercises.length) {
            nextWorkoutIndex = 0;
        }

        console.log($scope.currentWorkout.exercises.length);
        console.log('next workout is ' + nextWorkoutIndex);

        $scope.selectExercise($scope.currentWorkout.exercises[nextWorkoutIndex]);
    };

    $scope.selectExercise = function(exercise) {
        $scope.currentExercise = exercise;
    }

    $scope.setWorkoutDay = function (workoutDay) {
        $http.post('/api/WorkoutDay/SetNextWorkoutDay', '"' + workoutDay.id.toString() + '"').success(function () {
            $scope.currentWorkout = workoutDay;
            $scope.originalWorkoutDay = workoutDay;
        });
    }

    $scope.finishWorkout = function() {
        $scope.currentExercise = null;
        $http.post('/api/WorkoutDay/CompleteWorkout', $scope.currentWorkout);
    };

    $http.get('/api/WorkoutDay/GetCompletedWorkouts?take=10&skip=0').success(function(completedWorkouts) {
        $scope.completedWorkouts = completedWorkouts;
    });

    $http.get('/api/Program/GetUserExercises').success(function (exercises) {
        $scope.userExercises = new Array();

        for (var i = 0; i < exercises.length; i++) {
            $scope.userExercises[exercises[i].exercise.name] = exercises[i];
        }
    });

    $scope.editExercise = function (exercise) {
        $scope.selectedExercise = exercise;
        
        $mdDialog.show({
            controller: DialogController,
            templateUrl: 'dialog1.tmpl.html',
            parent: angular.element(document.body),
            //targetEvent: ev,
            clickOutsideToClose: true,
            locals: { exercise: $scope.selectedExercise }
        })
        .then(function (orm) {
            if ($scope.userExercises[$scope.selectedExercise.exercise.name] == null) {
                console.log('setting userexercises to ' + $scope.selectedExercise.exercise.name + ' ' + orm);

                $scope.userExercises[$scope.selectedExercise.exercise.name] = {
                    exercise: $scope.selectedExercise.exercise,
                    oneRepetitionMax: orm
                };
            }

            $scope.userExercises[$scope.selectedExercise.exercise.name].oneRepetationMax = orm;

            $http.post('/api/Exercise/UpdateUserExercise', $scope.userExercises[$scope.selectedExercise.exercise.name]);
            //TODO: this

        }, function () {
            $scope.status = 'You cancelled the dialog.';
        });
    };
});


function DialogController($scope, $mdDialog, exercise) {
    $scope.exercise = exercise;

    $scope.hide = function () {
        $mdDialog.hide();
    };

    $scope.cancel = function () {
        $mdDialog.cancel();
    };

    $scope.setWeight = function (weight) {
        $mdDialog.hide(weight);
    };
}

angular.module('main').controller('programController', function ($scope, $http, programUpdater) {
  
    $scope.newSet = {};
    $scope.newExerciseName = "";

    function guid() {
        function s4() {
            return Math.floor((1 + Math.random()) * 0x10000)
              .toString(16)
              .substring(1);
        }

        return s4() + s4() + '-' + s4() + '-' + s4() + '-' +
          s4() + '-' + s4() + s4() + s4();
    }

    $scope.selectExercise = function (exercise) {
        if ($scope.selectedExercise == exercise) {
            //deselect
            $scope.selectedExercise = null;
        } else {
            $scope.selectedExercise = exercise;
        }
    };

    $scope.selectDay = function(day) {
        $scope.selectedDay = day;
    };

    $scope.saveEverything = function() {
        programUpdater.saveProgram($scope.currentProgram).success(function() {
            alert('yay');
        }).error(function() {
            alert('nein');
        });
    };

    $scope.addDay = function () {
        var workoutDay = {
            order: $scope.currentProgram.workoutDays.length + 1,
            id: guid()
        };

        $http.post('/api/WorkoutDay/Create', { workoutDay: workoutDay, program: $scope.currentProgram })
            .success(function () {
                if ($scope.selectedDay == null) {
                    $scope.selectedDay = workoutDay;
                }

                $scope.selectedExerciseToAdd = null;
                $scope.currentProgram.workoutDays.push(workoutDay);
                loadProgram();
            });
    };

    $scope.addExercise = function (exercise) {
        var workoutExercise = {
            id: guid(),
            exercise: exercise,
            sets: []
    };

        console.log($scope.selectedDay);

        $http.post('/api/WorkoutExercise/Create', { workoutDay: $scope.selectedDay, exercise: workoutExercise })
            .success(function () {
                if ($scope.selectedDay.workoutDays == null)
                    $scope.selectedDay.workoutDays = [];

                $scope.selectedDay.exercises.push(workoutExercise);
                $scope.selectedExerciseToAdd = null;
                $scope.addingExercise = false;
            });
    };

    $scope.removeExercise = function (exercise) {
        $http.delete('/api/WorkoutExercise/Delete?id=' + exercise.id)
            .success(function () {
                var removeIndex = $scope.selectedDay.exercises.indexOf(exercise);
                $scope.selectedDay.exercises.splice(removeIndex, 1);
        });
    };

    $scope.createExercise = function (name) {
        var newExercise = {
            id: guid(),
            name: name
        };

        if (newExercise.name == null) {
            alert('name must be set');
            return;
        }

        $http.post('/api/Exercise/Create', newExercise).success(function () {
            $scope.allExercises.push(newExercise);
            $scope.newExerciseName = "";
        });
    };

    $scope.addSet = function() {
        $scope.newSet.id = guid();
        $scope.newSet.order = $scope.selectedExercise.sets.length + 1;

        $http.post('/api/Set/Create', { Exercise: $scope.selectedExercise,  Set: $scope.newSet }).success(function () {
            $scope.selectedExercise.sets.push($scope.newSet);
            $scope.newSet = {};
        });
    };

    $scope.removeSet = function (set) {
        $http.delete('/api/Set/Delete?id=' + set.id).success(function() {
            var indexOfSet = $scope.selectedExercise.sets.indexOf(set);
            $scope.selectedExercise.sets.splice(indexOfSet, 1);
        }).error(function() {
        });
    };

    $scope.updateSet = function (set) {
        $http.put('/api/Set/Update', set ).success(function () {
        }).error(function() {
            alert('couldnt update set');
        });
    };

    $scope.removeDay = function (day) {
        $http.delete('/api/WorkoutDay/Delete?id=' + day.id).success(function () {
            var index = $scope.currentProgram.workoutDays.indexOf(day);
            $scope.currentProgram.workoutDays.splice(index, 1);
        }).error(function () {
        });
    };

    function loadExercises() {
        console.log('loading exercises');
        $http.get('/api/Exercise/GetAll').success(function (exercises) {
            $scope.allExercises = exercises;
        });
    }

    function loadProgram() {
        $http.get('/api/Program/GetCurrentProgram').success(function (program) {
            $scope.currentProgram = program;

            if ($scope.currentProgram.workoutDays != null) {
                $scope.selectedDay = $scope.currentProgram.workoutDays[0];
            }
        });
    }

    $scope.sortingLog = [];

    $scope.sortableOptions = {
        activate: function () {
            console.log("activate");
        },
        beforeStop: function () {
            console.log("beforeStop");
        },
        change: function () {
            console.log("change");
        },
        create: function () {
            console.log("create");
        },
        deactivate: function () {
            console.log("deactivate");
        },
        out: function () {
            console.log("out");
        },
        over: function () {
            console.log("over");
        },
        receive: function () {
            console.log("receive");
        },
        remove: function () {
            console.log("remove");
        },
        sort: function () {
            console.log("sort");
        },
        start: function () {
            console.log("start");
        },
        update: function (e, ui) {
            console.log("update");

            //var logEntry = tmpList.map(function (i) {
            //    return i.value;
            //}).join(', ');
            //$scope.sortingLog.push('Update: ' + logEntry);
        },
        stop: function (e, ui) {
            console.log("stop");

            for (var i = 0; i < $scope.selectedExercise.sets.length; i++) {
                var set = $scope.selectedExercise.sets[i];
                set.order = i;
                $scope.updateSet(set);
            }
        }
    };

    loadProgram();
    loadExercises();
});


angular.module('main').controller('programCreatorController', function ($scope, $http) {
    $scope.programSet = false;

    //get available programs
    $http.get('/api/Program/GetAll').success(function(programs) {
        $scope.allPrograms = programs;
    });

    $scope.selectProgram = function() {
        $http.post('/api/Program/SelectProgram', { program: $scope.selectedProgram })
            .success(function () {
                $scope.selectedProgram = null;
            $scope.programSet = true;
        });
    };


    $scope.createNewProgram = function() {
        $scope.newProgram = {
            Name: "Unnamed program"
        };
    };

    $scope.create = function () {
        console.log('creating stuff');

        $http.post('/api/Program/CreateUpdateProgram', program).success(function() {
            alert('ficlem created');

            $scope.creatingNew = false;
        });
    };
});


angular.module('main').controller('programEditorController', function ($scope, $http) {
    $scope.programSet = false;

    //get available programs
    $http.get('/api/Program/GetAll').success(function(programs) {
        $scope.allPrograms = programs;
    });

    $scope.selectProgram = function() {
        $http.post('/api/Program/SelectProgram', { program: $scope.selectedProgram })
            .success(function () {
                $scope.selectedProgram = null;
            $scope.programSet = true;
        });
    };


    $scope.createNewProgram = function() {
        $scope.newProgram = {
            Name: "Unnamed program"
        };
    };
});


angular.module('main').controller('userExerciseController', function ($scope, $http, $mdDialog) {

    //$scope.save = function() {
    //    $http.post('/api/Exercise/UpdateUserExercise?')
    //};

    $scope.status = '  ';

    $scope.showAdvanced = function (ev) {
        $mdDialog.show({
            controller: DialogController,
            templateUrl: 'dialog1.tmpl.html',
            parent: angular.element(document.body),
            targetEvent: ev,
            clickOutsideToClose: true
        })
        .then(function (answer) {
            $scope.status = 'You said the information was "' + answer + '".';
        }, function () {
            $scope.status = 'You cancelled the dialog.';
        });
    };
});


angular.module('main').controller('userWeight', function ($scope, $http, $mdDialog) {
    $scope.registerWeight = function () {
        console.log('posting');
        $http.post('/api/UserWeight/Register?amount=' + $scope.userWeight).success(function(weight) {
            $scope.registeredWeights.push(weight);
        });

        $scope.userWeight = null;
        $scope.calculateGains();
    };

    $scope.deleteWeight = function(weight) {
        $http.delete('/api/UserWeight/Delete?id=' + weight.id).success(function() {
            var index = $scope.registeredWeights.indexOf(weight);
            $scope.registeredWeights.splice(index, 1);
        });
    };

    $scope.calculateGains = function() {
        $http.get('/api/UserWeight/GetWeightStats').success(function (stats) {
            $scope.stats = stats;
        });
    };

    $http.get('/api/UserWeight/GetAll').success(function (weights) {
        $scope.registeredWeights = weights;

        $scope.calculateGains();
    });
});

