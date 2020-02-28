appControllers.controller('TestWebAPIController', function ($scope, CommonService, DashboardService, projectService) {
    this.$inject = ['$scope', 'CommonService', 'DashboardService', 'projectService'];

    $scope.offerID = 1;
    $scope.projectID = 1;
    $scope.mediaProjectID = 1;
    $scope.exportProjectID = 1;
    $scope.folderPath = '/MediaLibrary/Public';
    $scope.assetType = 'ALL';
    $scope.fileType = 'ALL';

    $scope.getOffers = function () {
        CommonService.getOffers(
            function (data) {
                console.log(data);
                alert(JSON.stringify(data));
            }, 
            function (data, status) {
                console.log("Error", data, status);
                alert("Error. Check the Console: " + data + " " + status);
            });
    };

    $scope.getOfferById = function () {
        CommonService.getOfferById($scope.offerID,
                function (data) {
                    console.log(JSON.stringify(data));
                    alert(JSON.stringify(data));
                },
                function (data, status) {
                    console.log("Error", data, status);
                    alert("Error. Check the Console: " + data + " " + status);
                });
    };
    
    $scope.getAvailableOffers = function () {
        CommonService.getAvailableOffers(
                function (data) {
                    console.log(JSON.stringify(data));
                    alert(JSON.stringify(data));
                },
                function (data, status) {
                    console.log("Error", data, status);
                    alert("Error. Check the Console: " + data + " " + status);
                });
    };

    $scope.getProjects = function () {
        projectService.getProjects(
                function (data) {
                    console.log(JSON.stringify(data));
                    alert(JSON.stringify(data));
                },
                function (data, status) {
                    console.log("Error", data, status);
                    alert("Error. Check the Console: " + data + " " + status);
                });
    };

    $scope.getProjectById = function () {
        projectService.getProjectById($scope.projectID,
                function (data) {
                    console.log(JSON.stringify(data));
                    alert(JSON.stringify(data));
                },
                function (data, status) {
                    console.log("Error", data, status);
                    alert("Error. Check the Console: " + data + " " + status);
                });
    };

    
    var projectToCreateData = {
        "ID": 1,
        "Approved": false,
        "HasProject": false,
        "CreateByExternal": null,
        "Layouts": [
            {
                "ID": 1,
                "Sections": [
                    {
                        "ID": 1,
                        "Components": [
                            {
                                "Inactive": false,
                                "Data": {}
                            }
                        ]
                    }
                ]
            },
            {
                "ID": 2,
                "Sections": [
                    {
                        "ID": 1,
                        "Components": [
                            {
                                "Inactive": false,
                                "Data": {}
                            }
                        ]
                    }
                ]
            }
        ]
    };

    $scope.createProject = function () {
        CommonService.createProject(projectToCreateData,
                function (data) {
                    console.log(JSON.stringify(data));
                    alert(JSON.stringify(data));
                },
                function (data, status) {
                    if (status == 409) {
                        alert("A project with this ID already exists");
                    }
                    else {
                        console.log("Error", data, status);
                        alert("Error. Check the Console: " + data + " " + status);
                    }
                });
    };

    var projectToUpdateData = {
        "ID": 1,
        "Approved": true,
        "Layouts": [
            {
                "ID": 1,
                "Sections": [
                    {
                        "ID": 1,
                        "Components": [
                            {
                                "TypeID": 1,
                                "Inactive": false,
                                "Data": "test data"
                            },
                            {
                                "TypeID": 2,
                                "Inactive": false,
                                "Data": "test2 data"
                            }
                        ]
                    },
                    {
                        "ID": 2,
                        "Components": [
                            {
                                "TypeID": 1,
                                "Inactive": false,
                                "Data": "test data"
                            },
                            {
                                "TypeID": 2,
                                "Inactive": false,
                                "Data": "test2 data"
                            }
                        ]
                    }
                ]
            },
            {
                "ID": 2,
                "Sections": [
                    {
                        "ID": 1,
                        "Components": [
                            {
                                "TypeID": 1,
                                "Inactive": false,
                                "Data": "test data"
                            }
                        ]
                    }
                ]
            }
        ]
    };

    $scope.updateProject = function () {
        projectService.updateProject(projectToUpdateData)
            .success(function(data){
                console.log(JSON.stringify(data));
                alert("Successfully Updated");            
            })
            .error(function() {
                if (status == 404) {
                    alert("The project doesn't exists ");
                }
                else {
                    console.log("Error", data, status);
                    alert("Error. Check the Console: " + data + " " + status);
                }            
            });
    };

    $scope.deleteProject = function () {
        projectService.deleteProject(1,
                function (data) {
                    console.log(JSON.stringify(data));
                    alert("Successfully Deleted");
                },
                function (data, status) {
                    if (status == 404) {
                        alert("The project doesn't exists or was previously deleted");
                    }
                    else {
                        console.log("Error", data, status);
                        alert("Error. Check the Console: " + data + " " + status);
                    }
                });
    };

    $scope.changeProjectStatus = function () {
        projectService.changeProjectStatus(1, true)
            .success(function (data) {
                console.log(JSON.stringify(data));
                alert("Successfully Changed");
            })
            .error(function (data, status) {
                if (status == 404) {
                    alert("The project doesn't exists or was previously deleted");
                }
                else {
                    console.log("Error", data, status);
                    alert("Error. Check the Console: " + data + " " + status);
                }
            });
    };

    $scope.getImagesForProject = function () {
        CommonService.getImagesForProject($scope.mediaProjectID)
            .success(function (data) {
                console.log(JSON.stringify(data));
                alert(JSON.stringify(data));
            })
            .error(function (data, status) {
                console.log("Error", data, status);
                alert("Error. Check the Console: " + data + " " + status);
            });
    };
    
    $scope.getAllAssets = function () {
        var json = { "filter": $scope.assetType };

        CommonService.getAllAssets(json)
            .success(function (data) {
                console.log(JSON.stringify(data));
                alert(JSON.stringify(data));
            })
            .error(function (data, status) {
                console.log("Error", data, status);
                alert("Error. Check the Console: " + data + " " + status);
            });
    };

    $scope.getAssetsInFolder = function () {
        var json = { "path": $scope.folderPath, "filter": $scope.fileType };

        CommonService.getAssetsInFolder(json)
            .success(function (data) {
                console.log(JSON.stringify(data));
                alert(JSON.stringify(data));
            })
            .error(function (data, status) {
                console.log("Error", data, status);
                alert("Error. Check the Console: " + data + " " + status);
            });
    };

});