angular.module('main').controller('routineController', function($scope, $http) {
    $scope.lastSetWorkout = 0;
    var currentWorkoutDay = 0;
    var currentExercise = 0;
    $scope.currentProgram = {};

    $scope.getNextWorkout = function () {
        $http.get('/api/WorkoutDay/GetNextWorkoutDay').success(function (workoutDay) {
            $scope.currentWorkout = workoutDay;
            $scope.currentExercise = workoutDay.exercises[0];
        });
    };


    $scope.getNextWorkout();

    $scope.finishExercise = function() {
        currentExercise++;

        if (currentExercise >= $scope.currentWorkout.exercises.length)
            currentExercise = 0;

        $scope.currentExercise = $scope.currentWorkout.exercises[currentExercise];
    };

    $scope.finishWorkout = function() {
        $scope.currentExercise = null;
        $http.post('/api/WorkoutDay/CompleteWorkout', $scope.currentWorkout);
    };

    $http.get('/api/WorkoutDay/GetCompletedWorkouts?take=10&skip=0').success(function(completedWorkouts) {
        $scope.completedWorkouts = completedWorkouts;
    });
    //$http.get('/api/Program/GetUserPrograms').success(function (programs) {
    //    $scope.programs = programs;
    //    $scope.currentWorkout = programs[0].workoutDays[currentWorkoutDay];
    //    $scope.currentProgram = programs[0];

    //    $scope.currentExercise = programs[0].workoutDays[currentWorkoutDay].exercises[0];
    //    console.log($scope.currentExercise);
    //});

    $http.get('/api/Program/GetUserExercises').success(function (exercises) {
        $scope.exercises = new Array();

        for (var i = 0; i < exercises.length; i++) {
            $scope.exercises[exercises[i].exercise.name] = exercises[i].oneRepetationMax;
        }
    });

    //$scope.markAsDone = function(dayExercise) {
    //    $http.post('/api/Program/MarkExerciseAsCompleted', dayExercise);
    //};
});