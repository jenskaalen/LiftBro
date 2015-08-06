angular.module('main').controller('routineController', function ($scope, $http) {
    $scope.lastSetWorkout = 0;
    var currentWorkoutDay = 0;
    var currentExercise = 0;

    $scope.changeWorkout = function() {
        $scope.lastSetWorkout++;

        if ($scope.lastSetWorkout >= $scope.programs[0].workoutDays.length + 1)
            $scope.lastSetWorkout = 0;

        $scope.currentExercise = $scope.programs[0].workoutDays[$scope.lastSetWorkout].exercises[0];
    };

    $scope.finishExercise = function() {
        currentExercise++;

        if (currentExercise >= $scope.programs[0].workoutDays[currentWorkoutDay].exercises.length)
            currentExercise = 0;

        $scope.currentExercise = $scope.programs[0].workoutDays[currentWorkoutDay].exercises[currentExercise];
    };

    $http.get('/api/Program/GetUserPrograms').success(function (programs) {
        $scope.programs = programs;
        $scope.currentWorkout = programs[0].workoutDays[currentWorkoutDay];

        $scope.currentExercise = programs[0].workoutDays[currentWorkoutDay].exercises[0];
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