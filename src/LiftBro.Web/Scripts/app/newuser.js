angular.module('main').controller('newUserController', function ($scope, $http) {
    $scope.selectProgram = function () {
        $http.post('/api/Program/SelectProgram', $scope.selectedProgram)
            .success(function () {
                $scope.selectedProgram = null;
                $scope.programSet = true;
            });
    };

    $scope.loadPrograms = function () {
        $http.get('/api/Program/GetAll').success(function (programs) {
            $scope.allPrograms = programs;
        });
    };


    $scope.createNewProgram = function () {
        $scope.creatingNew = true;
        $scope.newProgram = {
            Name: "Unnamed program",
        };
    };

    $scope.create = function () {
        console.log('creating stuff');

        $http.post('/api/Program/CreateUpdateProgram', $scope.newProgram).success(function () {
            $scope.creatingNew = false;
            $scope.loadPrograms();
        });
    };

    $scope.programSet = false;
    $scope.loadPrograms();
});

