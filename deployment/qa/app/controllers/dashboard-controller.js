appControllers.controller('DashboardController', function ($rootScope, $scope, $filter, DashboardService, projectService) {
    $scope.dashboardVM = null;
    $scope.enableActiveProjects = true;
    $scope.enableArchivedProjects = true;
    $scope.enableUserList = true;

    $scope.tbUsersModel = null;
    $scope.tbActiveProjectsModel = null;
    $scope.tbArchivedProjectsModel = null;

    $scope.exportMenuVisible = false;

    $scope.toggleExportMenu = function () {
        $scope.exportMenuVisible = !$scope.exportMenuVisible;
    };

    $scope.isProjectSelectionDlgVisible = false;

    $scope.openProjectSelectionDlg = function (title, description, data, onYesClick) {
        $scope.isProjectSelectionDlgVisible = true;
        $scope.$parent.areScrollsHidden = true;

        $scope.$broadcast(
            'projects-selection-dlg:opened',
            {
                'title': title,
                'description': description,
                'data': data,
                'onYesClick': onYesClick,
                'onCancelClick': $scope.closeProjectSelectionDlg
            });
    };

    $scope.closeProjectSelectionDlg = function () {
        $scope.$parent.areScrollsHidden = false;
        $scope.isProjectSelectionDlgVisible = false;
    };

    $rootScope.$on('project:inserted', function () {
        $scope.loadData();
    });

    $rootScope.$on('offer:deleted', function () {
        $scope.loadData();
    });

    $rootScope.$on('offerEdited', function () {
        $scope.loadData();
    });

    $scope.toggleActiveProjects = function () {
        $scope.enableActiveProjects = !$scope.enableActiveProjects;
    };

    $scope.toggleArchivedProjects = function () {
        $scope.enableArchivedProjects = !$scope.enableArchivedProjects;
    };

    $scope.toggleUserList = function () {
        $scope.enableUserList = !$scope.enableUserList;
    };

    $scope.gotoEditProject = function (ID) {
        $scope.gotoPage('Home/OfferProjectLayout#/layout-menu/', ID);
    };

    $scope.archiveSelected = function () {
        var selectedProjects = $scope.tbActiveProjectsModel.getSelectedItems();

        if (selectedProjects.length === 0) {
            $scope.$parent.openInformationDialogClick("Error", 'You must select at least one ProjectID!');
            return;
        }

        $scope.openProjectSelectionDlg(
            "Move Project(s) to Archive",
            "Are you sure you want to archive the following projects?",
            selectedProjects,
            function () {
                var ids = getIDsAsArray(selectedProjects);
                projectService.moveProjectsTo("archive", ids)
                    .success(function () {
                        $scope.tbActiveProjectsModel.unselectAll();
                        $scope.loadData();
                    })
                    .error(function () {
                        $scope.$parent.openInformationDialogClick("Error", 'An error has occurred, please refresh the page or contact support for assistance!');
                        console.log("Error", data, status);
                        $scope.loadData();
                    });

                $scope.closeProjectSelectionDlg();
            });
    };

    $scope.isAdmin = function () {
        if (CURRENT_ROL === "Administrator") {
            return true;
        }

        return false;
    };

    $scope.isSuperAdmin = function () {
        console.log("CURRENT_ROL ", CURRENT_ROL);
        if (CURRENT_ROL === "Super Admin") {
            return true;
        }

        return false;
    }

    $scope.exportSelectedToZip = function () {
        $scope.toggleExportMenu();
        var selectedProjects = $scope.tbActiveProjectsModel.getSelectedItems();

        if (selectedProjects.length === 0) {
            $scope.$parent.openInformationDialogClick("Error", 'You must select at least one ProjectID!');
            return;
        }

        $scope.openProjectSelectionDlg(
            "Export for Web Team",
            "Export the following Offers",
            selectedProjects,
            function () {
                var ids = getIDsAsArray(selectedProjects);
                $scope.tbActiveProjectsModel.unselectAll();
                post(BASE_URL + "/Project/ExportProjects/", { "pids": ids });
                $scope.closeProjectSelectionDlg();
            });
    };

    $scope.exportSelectedToPDF = function () {
        $scope.toggleExportMenu();
        var selectedProjects = $scope.tbActiveProjectsModel.getSelectedItems();

        if (selectedProjects.length === 0) {
            $scope.$parent.openInformationDialogClick("Error", 'You must select at least one ProjectID!');
            return;
        }

        $scope.openProjectSelectionDlg(
            "Export for Review",
            "Export the following Offers",
            selectedProjects,
            function () {
                var ids = getIDsAsArray(selectedProjects);
                var totalContIDs = getTotalContntIDs(selectedProjects);

                if (totalContIDs <= MAX_PDF_NUMBER_ON_THE_FLY) {
                    post(BASE_URL + "/PDF/DownloadAsZip/", { "pids": ids });
                }
                else {
                    projectService.exportProjectsForReviewAsync(ids)
                        .success(function (result) {
                            $scope.tbActiveProjectsModel.unselectAll();
                            $scope.$parent.openInformationDialogClick("Info", 'We are processing your request and will send you an email when your package is ready.');
                        })
                        .error(function (result, code) {
                            $scope.$parent.openInformationDialogClick("Error", 'An error has occurred, please refresh the page or contact support for assistance!');
                            console.log("Error", data, status);
                        });
                }
                $scope.closeProjectSelectionDlg();
            });
    };


    $scope.activateSelected = function () {
        var selectedProjects = $scope.tbArchivedProjectsModel.getSelectedItems();

        if (selectedProjects.length === 0) {
            $scope.$parent.openInformationDialogClick("Error", 'You must select at least one ProjectID!');
            return;
        }

        $scope.openProjectSelectionDlg(
            "Move Project(s) to Active",
            "Are you sure you want to activate the following projects?",
            selectedProjects,
            function () {
                var ids = getIDsAsArray(selectedProjects);

                projectService.moveProjectsTo("active", ids)
                    .success(function () {
                        $scope.tbArchivedProjectsModel.unselectAll();
                        $scope.loadData();
                    })
                    .error(function () {
                        $scope.$parent.openInformationDialogClick("Error", 'An error has occurred, please refresh the page or contact support for assistance!');
                        console.log("Error", data, status);
                        $scope.loadData();
                    });

                $scope.closeProjectSelectionDlg();
            });
    };

    function getIDsAsArray(projects) {
        var ids = [];
        for (var i = 0; i < projects.length; i++) {
            ids.push(projects[i].OfferID);
        }

        return ids;
    }

    function getTotalContntIDs(projects) {
        var count = 0;
        for (var i = 0; i < projects.length; i++) {
            var cIds = projects[i].ContentIDs.split(",");;
            count = count + cIds.length;
        }
        return count;
    }

    $scope.sortBy = function (model, value) {
        if (model === 'tbActiveProjectsModel') {
            $scope.tbActiveProjectsModel.sortBy(value);
        } else if (model === 'tbArchivedProjectsModel') {
            $scope.tbArchivedProjectsModel.sortBy(value);
        } else if (model == 'tbUsersModel') {
            $scope.tbUsersModel.sortBy(value);
        }
    };

    $scope.createNewUser = function () {
        $scope.$parent.createNewUser();
    };

    $scope.openUserProfile = function (userId) {
        $scope.$parent.goto('Account/UserProfile/#/?userId=' + userId);
    };

    $scope.loadData = function () {
        
        if ($scope.isSuperAdmin()) {

            DashboardService.getUsers()
                .success(function (users) {
                    $scope.tbUsersModel = new TableModel(users, $filter('orderBy'));
                    $scope.tbUsersModel.sortBy('DateCreated', true);
                })
                .error(function (data, status) {
                    console.log("Error", data, status);
                });
   
            DashboardService.getActiveProjects()
                .success(function (projects) {
                    $scope.tbActiveProjectsModel = new TableModel(projects, $filter('orderBy'));
                    $scope.tbActiveProjectsModel.sortBy('DateCreated', true);
                })
                .error(function (data, status) {
                    console.log("Error", data, status);
                });

            DashboardService.getArchivedProjects()
                .success(function (projects) {
                    $scope.tbArchivedProjectsModel = new TableModel(projects, $filter('orderBy'));
                    $scope.tbArchivedProjectsModel.sortBy('DateCreated', true);
                })
                .error(function (data, status) {
                    $scope.$parent.openInformationDialogClick("Error", 'An error has occurred, please refresh the page or contact support for assistance!');
                    console.log("Error", data, status);
                });

        } else {
            DashboardService.getUsersByUsersType(CURRENT_USER_TYPE_ID)
                .success(function (users) {
                    $scope.tbUsersModel = new TableModel(users, $filter('orderBy'));
                    $scope.tbUsersModel.sortBy('DateCreated', true);
                })
                .error(function (data, status) {
                    console.log("Error", data, status);
                });

            DashboardService.getActiveProjectsByOfferType(CURRENT_USER_TYPE_ID)
                .success(function (projects) {
                    $scope.tbActiveProjectsModel = new TableModel(projects, $filter('orderBy'));
                    $scope.tbActiveProjectsModel.sortBy('DateCreated', true);
                })
                .error(function (data, status) {
                    console.log("Error", data, status);
                });

            DashboardService.getArchivedProjectsByOfferType(CURRENT_USER_TYPE_ID)
                .success(function (projects) {
                    $scope.tbArchivedProjectsModel = new TableModel(projects, $filter('orderBy'));
                    $scope.tbArchivedProjectsModel.sortBy('DateCreated', true);
                })
                .error(function (data, status) {
                    $scope.$parent.openInformationDialogClick("Error", 'An error has occurred, please refresh the page or contact support for assistance!');
                    console.log("Error", data, status);
                });
        }
        
    };

    $scope.init = function () {
        $scope.loadData();
    };

    $scope.init();
});