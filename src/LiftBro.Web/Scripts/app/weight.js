angular.module('main').controller('userWeight', function ($scope, $http, $mdDialog) {
    $scope.registerWeight = function () {
        console.log('posting');
        $http.post('/api/UserWeight/Register?amount=' + $scope.userWeight).success(function(weight) {
            $scope.registeredWeights.push(weight);
        });

        $scope.userWeight = null;
        $scope.calculateGains();
    };

    $scope.deleteWeight = function(weight) {
        $http.delete('/api/UserWeight/Delete?id=' + weight.id).success(function() {
            var index = $scope.registeredWeights.indexOf(weight);
            $scope.registeredWeights.splice(index, 1);
        });
    };

    $scope.calculateGains = function() {
        $http.get('/api/UserWeight/GetWeightStats').success(function (stats) {
            $scope.stats = stats;
        });
    };

    $http.get('/api/UserWeight/GetAll').success(function (weights) {
        $scope.registeredWeights = weights;

        $scope.calculateGains();
    });
});

