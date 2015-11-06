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
