angular.module('main').controller('programController', function ($scope, $http) {
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
                $scope.selectedExerciseToAdd = null;
                loadProgram();
        });
    };

    $scope.removeExercise = function (exercise) {
        $http.post('/api/WorkoutDay/UpdateExercise', { workoutDay: $scope.selectedDay, exercise: exercise, modifier: 'delete' })
            .success(function () {
                //is this ok?
                var removeAt = $scope.selectedDay.exercises.indexOf(exercise);
                $scope.selectedDay.exercises.splice(removeAt, 1);
        });
    };

    $scope.addSet = function() {
        $scope.newSet.id = guid();
        $scope.newSet.order = $scope.selectedExercise.sets.length + 1;

        $http.post('/api/WorkoutExercise/UpdateSet', { exercise: $scope.selectedExercise, set: $scope.newSet, modifier: 'add' }).success(function () {
            $scope.selectedExercise.sets.push($scope.newSet);
            $scope.newSet = { ormPercentage: 50 };
        });
    };

    $scope.removeSet = function(set) {
        $http.post('/api/WorkoutExercise/UpdateSet', { exercise: $scope.selectedExercise, set: set, modifier: 'delete' }).success(function() {
            var indexOfSet = $scope.selectedExercise.sets.indexOf(set);
            $scope.selectedExercise.sets.splice(indexOfSet, 1);
        });
    };

    $scope.updateSettings = function (set) {
        $http.post('/api/WorkoutExercise/UpdateSet', { exercise: $scope.selectedExercise, set: set, modifier: 'update' }).success(function () {
            var indexOfSet = $scope.selectedExercise.sets.indexOf(set);
            $scope.selectedExercise.sets.splice(indexOfSet, 1);
        });
    };

    function guid() {
        var guid = (S4() + S4() + "-" + S4() + "-4" + S4().substr(0, 3) + "-" + S4() + "-" + S4() + S4() + S4()).toLowerCase();
        return guid;
    }

    function S4() {
        return (((1 + Math.random()) * 0x10000) | 0).toString(16).substring(1);
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
        });
    }
});

