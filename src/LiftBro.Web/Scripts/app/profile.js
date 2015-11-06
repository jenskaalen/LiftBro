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
