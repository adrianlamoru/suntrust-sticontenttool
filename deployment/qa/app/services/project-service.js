
appServices.service('projectService', function ($http) {
    
    this.getProjects = function () {
        return $http.get(BASE_URL + '/api/Project');
    };

    this.getProjectById = function (id) {
        return $http.get(BASE_URL + '/api/Project/' + id);
    };

    this.getProjectLayoutStaticViewModel = function (id) {
        var url = BASE_URL + '/api/project??/' + id;

        return $http.get(url);
    };

    this.updateProject = function (pData) {
        return $http.post(BASE_URL + '/api/Project/Update/' + pData.ID, pData);
    };

    this.moveProjectsTo = function (status, pData) {
        return $http.post(BASE_URL + '/api/Project/MoveProjectsTo/' + status, pData);
    };

    this.deleteProject = function (id) {
        $http.post(BASE_URL + '/api/Project/Delete/' + id);
    };

    this.changeProjectStatus = function (id, status) {
        return $http.post(BASE_URL + '/api/Project/Aproved/' + id + '/' + status);
    };

    this.exportProjectsForReviewAsync = function (ids) {
        var data = "pids=" + ids;

        var config = {
            'headers': {
                'Content-Type': 'application/x-www-form-urlencoded'
            }
        };

        return $http.post(BASE_URL + '/PDF/DownloadAsZip/', data, config);
    };

    this.exportProjectContents = function (projectId) {
        window.location.href = BASE_URL + '/Project/ExportContents/' + projectId;
    };

    // begin: layout details
    this.getLayoutDetail = function (id) {
        return $http.get(BASE_URL + '/api/LayoutDetails/' + id);
    };

    this.updateLayoutDetail = function (id, layoutDetail) {
        return $http.put(BASE_URL + '/api/LayoutDetails/' + id, layoutDetail);
    };

    this.addLayoutDetail = function (layoutDetail) {
        return $http.post(BASE_URL + '/api/LayoutDetails/', layoutDetail);
    };

    this.deleteLayoutDetail = function (id) {
        return $http.delete(BASE_URL + '/api/LayoutDetails/' + id);
    };

    // end: layout details
});