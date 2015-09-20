angular.module('main').controller('programController', function ($scope, $http, programUpdater) {
    loadProgram();
    loadExercises();

    $scope.newSet = {};

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
            order: $scope.currentProgram.workoutDays.length,
            id: guid()
        };

        $http.post('/api/WorkoutDay/UpdateDay', { workoutDay: workoutDay, program: $scope.currentProgram, modifier: 'add' })
            .success(function () {
                $scope.selectedExerciseToAdd = null;
                $scope.currentProgram.workoutDays.push(workoutDay);
                loadProgram();
            });
    };

    $scope.addExercise = function (exercise) {
        var workoutExercise = {
            id: guid(),
            exercise: exercise
        };

        $http.post('/api/WorkoutExercise/Create', { workoutDay: $scope.selectedDay, exercise: workoutExercise })
            .success(function () {
                $scope.selectedExerciseToAdd = null;
                loadProgram();
            });
    };

    $scope.removeExercise = function (exercise) {
        $http.delete('/api/WorkoutExercise/Delete?id=' + exercise.id)
            .success(function () {
                var removeIndex = $scope.selectedDay.exercises.indexOf(exercise);
                $scope.selectedDay.exercises.splice(removeIndex, 1);
        });
    };

    //updating... order?

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
            console.log(program);
            //
        });
    }
});

