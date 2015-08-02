angular.module('main').controller('programController', function ($scope, $http) {
    $http.get('/api/Program/GetCurrentProgram').success(function(program) {
        $scope.currentProgram = program;
        console.log(program);
    });

    $http.get('/api/Exercise/GetAll').success(function (exercises) {
        $scope.allExercises = exercises;
    });

    $scope.newSet = {};

    $scope.selectExercise = function(exercise) {
        $scope.selectedExercise = exercise;
        console.log($scope.selectedExercise);
    };

    $scope.selectDay = function(day) {
        $scope.selectedDay = day;
    };

    $scope.addDay = function () {
        var workoutDay = new {
            order: $scope.currentProgram.workoutDays.length,
            id: guid()
                };

        $scope.currentProgram.workoutDays.push(workoutDay);
    };

    $scope.addExercise = function (exercise) {
        $http.post('/api/WorkoutDay/UpdateExercise', { workoutDay: $scope.selectedDay, exercise: exercise, modifier: 'add' })
            .success(function () {
                $scope.selectedDay.push($scope.selectedExerciseToAdd);
                $scope.selectedExerciseToAdd = null;
        });
    };

    $scope.addSet = function() {
        $scope.newSet.id = guid();
        $scope.newSet.order = $scope.selectedExercise.sets.length + 1;

        $http.post('/api/WorkoutExercise/UpdateSet', { exercise: $scope.selectedExercise, set: $scope.newSet, modifier: 0 }).success(function () {
            $scope.selectedExercise.sets.push($scope.newSet);
            $scope.newSet = null;
        });
    };

    function guid() {
        var guid = (S4() + S4() + "-" + S4() + "-4" + S4().substr(0, 3) + "-" + S4() + "-" + S4() + S4() + S4()).toLowerCase();
        return guid;
    }

    function S4() {
        return (((1 + Math.random()) * 0x10000) | 0).toString(16).substring(1);
    }
});

