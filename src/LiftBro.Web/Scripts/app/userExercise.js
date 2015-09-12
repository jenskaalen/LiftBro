angular.module('main').controller('userExerciseController', function ($scope, $http, $mdDialog) {

    //$scope.save = function() {
    //    $http.post('/api/Exercise/UpdateUserExercise?')
    //};

    $scope.status = '  ';

    $scope.showAdvanced = function (ev) {
        $mdDialog.show({
            controller: DialogController,
            templateUrl: 'dialog1.tmpl.html',
            parent: angular.element(document.body),
            targetEvent: ev,
            clickOutsideToClose: true
        })
        .then(function (answer) {
            $scope.status = 'You said the information was "' + answer + '".';
        }, function () {
            $scope.status = 'You cancelled the dialog.';
        });
    };
});

