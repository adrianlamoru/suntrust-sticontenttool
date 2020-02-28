appServices.service('DashboardService', function ($http) {
    this.getAll = function () {
        return $http.get(BASE_URL + '/api/dashboard');
    };

    this.getUsers = function () {
        return $http.get(BASE_URL + '/Api/Dashboard/Users');
    };

    this.getActiveProjects = function () {
        return $http.get(BASE_URL + '/Api/Dashboard/ActiveProjects');
    };

    this.getArchivedProjects = function () {
        return $http.get(BASE_URL + '/Api/Dashboard/ArchivedProjects');
    };
});
