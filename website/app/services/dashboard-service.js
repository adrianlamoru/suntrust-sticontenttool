appServices.service('DashboardService', function ($http) {
    this.getAll = function () {
        return $http.get(BASE_URL + '/api/dashboard');
    };

    this.getUsers = function () {
        return $http.get(BASE_URL + '/Api/Dashboard/Users');
    };

    this.getUsersByUsersType = function (userTypeId) {
        return $http.get(BASE_URL + '/Api/Dashboard/UsersByType/' + userTypeId);
    };

    this.getActiveProjects = function () {
        return $http.get(BASE_URL + '/Api/Dashboard/ActiveProjects');
    };

    this.getActiveProjectsByOfferType = function (offerTypeId) {
        return $http.get(BASE_URL + '/Api/Dashboard/ActiveProjectsByOfferType/' + offerTypeId);
    };

    this.getArchivedProjects = function () {
        return $http.get(BASE_URL + '/Api/Dashboard/ArchivedProjects');
    };

    this.getArchivedProjectsByOfferType = function (offerTypeId) {
        return $http.get(BASE_URL + '/Api/Dashboard/ArchivedProjectsByOfferType/' + offerTypeId);
    };
});
