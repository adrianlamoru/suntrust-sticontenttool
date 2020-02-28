appServices.service('CommonService', function ($http) {

    this.getOffers = function (callbackSuccess, errorCallback) {
        $http.get(BASE_URL + '/api/Offer')
            .success(function (data) {
                callbackSuccess(data);
            }).error(function (data, status) {
                errorCallback(data, status);
            });
    };

    this.getOfferById = function (id, callbackSuccess, errorCallback) {
        $http.get(BASE_URL + '/api/Offer/' + id)
            .success(function (data) {
                callbackSuccess(data);
            }).error(function (data, status) {
                errorCallback(data, status);
            });
    };

    this.getOfferForEdit = function (id, callbackSuccess, errorCallback) {
        $http.get(BASE_URL + '/api/Offer/Edit/' + id)
            .success(function (data) {
                callbackSuccess(data);
            }).error(function (data, status) {
                errorCallback(data, status);
            });
    };

    this.getAvailableOffers = function (callbackSuccess, errorCallback) {
        $http.get(BASE_URL + '/api/Offer/Availables')
            .success(function (data) {
                callbackSuccess(data);
            }).error(function (data, status) {
                errorCallback();
            });
    };

    this.createOffer = function (json, callbackSuccess, errorCallback) {
        $http.post(BASE_URL + '/api/Offer/Create', json)
            .success(function (data) {
                callbackSuccess(data);
            })
            .error(function (data, status) {
                errorCallback(data, status);
            });
    };

    this.saveOffer = function (json, callbackSuccess, errorCallback) {
        $http.post(BASE_URL + '/api/Offer/Save', json)
            .success(function (data) {
                callbackSuccess(data);
            })
            .error(function (data, status) {
                errorCallback(data, status);
            });
    };

    this.deleteOffer = function (id) {
        return $http.post(BASE_URL + "/api/Offer/Delete/" + id);
    }

    this.createProject = function (json, callbackSuccess, errorCallback) {
        $http.post(BASE_URL + '/api/Project/Create', json)
            .success(function (data, status) {
                callbackSuccess(data, status);
            }).error(function (data, status) {
                errorCallback(data, status);
            });
    };

    this.postUser = function (user) {
        return $http.post(BASE_URL + '/api/User/Post', user);
    }

    this.deleteUser = function (id) {
        return $http.post(BASE_URL + "/api/User/Delete/" + id);
    }

    //api / Offer / 1
    this.getUser = function (id) {
        return $http.get(BASE_URL + '/api/User/' + id);
    }

    this.getImagesForProject = function (projectId, callbackSuccess, errorCallback) {
        return $http.get(BASE_URL + '/api/Media/ImagesForProject/' + projectId);
    };

    //json.path, json.filter *optional
    this.getAssetsInFolder = function (json) {
        return $http.post(BASE_URL + '/api/Media/AssetsInFolder', json);
    };

    //json.filter *optional
    this.getAllAssets = function (json) {
        return $http.post(BASE_URL + '/api/Media/AllAssets', json);
    };

    //json.path 
    this.createMediaFolder = function (json) {
        return $http.post(BASE_URL + '/api/Media/CreateFolder', json);
    };

    //json.path 
    this.deleteMediaFolder = function (json) {
        return $http.post(BASE_URL + '/api/Media/DeleteFolder', json);
    };

    //json.path 
    this.deleteMediaFile = function (json) {
        return $http.post(BASE_URL + '/api/Media/DeleteFile', json);
    };

    //json = list 
    this.deleteBulkMediaAssets = function (json) {
        return $http.post(BASE_URL + '/api/Media/DeleteBulk', json);
    };

    //json = list 
    this.downloadBulkMediaAssets = function (json) {
        return $http.post(BASE_URL + '/Media/DownloadBulk', json);
    };

    this.searchInProjects = function (textToSearch) {
        return $http.get(BASE_URL + '/api/dashboard/search/' + textToSearch);
    };
});