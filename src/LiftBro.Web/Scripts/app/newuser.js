angular.module('main').controller('newUserController', function ($scope, $http) {
    $scope.programSet = false;

    //get available programs
    $http.get('/api/Program/GetAll').success(function(programs) {
        $scope.allPrograms = programs;
    });

    $scope.selectProgram = function() {
        $http.post('/api/Program/SelectProgram', { program: $scope.selectedProgram })
            .success(function () {
                $scope.selectedProgram = null;
            $scope.programSet = true;
        });
    };
});

