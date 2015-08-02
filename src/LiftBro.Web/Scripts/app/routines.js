angular.module('main').controller('routineController', function ($scope, $http) {
    $scope.lastSetWorkout = 0;

    $scope.changeWorkout = function() {
        $scope.lastSetWorkout++;

        if ($scope.lastSetWorkout >= $scope.routines[0].workoutDays.length)
            $scope.lastSetWorkout = 0;

        $scope.currentExercise = $scope.routines[0].workoutDays[$scope.lastSetWorkout].exercises[0];
    };

    $http.get('/api/Program/GetUserRoutines').success(function (routines) {
        $scope.routines = routines;
        $scope.currentWorkout = routines[0].workoutDays[0];

            $scope.currentExercise = routines[0].workoutDays[0].exercises[0];
            console.log($scope.currentExercise);
    });

    $http.get('/api/Program/GetUserExercises').success(function (exercises) {
        $scope.exercises = new Array();

        for (var i = 0; i < exercises.length; i++) {
            $scope.exercises[exercises[i].exercise.name] = exercises[i].oneRepetationMax;
        }
    });

    $scope.markAsDone = function(dayExercise) {
        $http.post('/api/Program/MarkExerciseAsCompleted', dayExercise);
    };
});