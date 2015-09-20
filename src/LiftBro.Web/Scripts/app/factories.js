
angular.module('main').factory('programUpdater', function($http) {
    return {
        saveProgram: function (program) {
            return $http.put('/api/Program/Update', program);
        }
    };
});
