
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