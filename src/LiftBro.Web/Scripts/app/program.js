angular.module('main').controller('programController', function ($scope, $http, programUpdater) {
  
    $scope.newSet = {};
    $scope.newExerciseName = "";

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

    function guid() {
        function s4() {
            return Math.floor((1 + Math.random()) * 0x10000)
              .toString(16)
              .substring(1);
        }

        return s4() + s4() + '-' + s4() + '-' + s4() + '-' +
          s4() + '-' + s4() + s4() + s4();
    }

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


            //// this callback has the changed model
            //var logEntry = tmpList.map(function (i) {
            //    return i.value;
            //}).join(', ');
            //$scope.sortingLog.push('Stop: ' + logEntry);
        }
    };

    loadProgram();
    loadExercises();
});

