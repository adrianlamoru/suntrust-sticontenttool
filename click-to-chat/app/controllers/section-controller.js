appControllers.controller('SectionController', function ($rootScope, $scope, $routeParams, $location, projectManager, projectService) {

    $scope.projectManager = projectManager;

    $scope.previewUrl = function () {
        return BASE_URL + "/app/partials/preview-dialog.html";
    };

    $scope.reset = function () {
        $rootScope.openConfimDialogClick('Reset Layout Editor', 'Resetting the Layout Editor will clear all adjustments and reset back to default setting. Are you sure you want to reset?',
            function () {
                //projectManager.setSelectedSection(projectManager.selectedSection.ID);
                projectManager.reset();

                var errors = projectManager.validateAndPrepareToSave();

                if (errors.length > 0) {
                    $scope.$parent.openInformationDialogClick("Errors saving the Project", errors.join('<br>'));
                    return;
                }

                projectService.updateProject(projectManager.project)
                    .success(function () {
                        $scope.refreshData(function () {
                            $scope.sectionForm.$setPristine();
                            var sectionID = projectManager.selectedSection.ID;
                            projectManager.selectedSection = {};
                            projectManager.setSelectedSection(sectionID);
                            $scope.$broadcast('project:reset');
                            $scope.$parent.openInformationDialogClick('Reset Layout Editor', 'The layout has been reset.');
                        });
                    })
                    .error(function (data, status) {
                        if (status == 404) {
                            $scope.$parent.openInformationDialogClick(null, "The project doesn't exists");
                        }
                        else {
                            $scope.$parent.openInformationDialogClick(null, 'An error has occurred, please try again or contact support for assistance!.');
                        }
                    });

            });
    };
        
    $scope.cancel = function () {
        if ($scope.sectionForm.$dirty) {
            $rootScope.openConfimDialogClick('Cancel Layout Editor', 'Canceling will revert all changes back to the previous save state. Are you sure you wish to cancel?',
                function () {
                    $scope.sectionForm.$setPristine();
                    $scope.showView('layout-menu');
                });
        } else {
            $scope.showView('layout-menu');
        }
        
    };

    $scope.save = function () {
        if ($scope.sectionForm.$valid) {
            $rootScope.openConfimDialogClick('Confirm', 'Are you sure you want to save the Project?', function () {
                saveProject();
            });
        }
    };

    var saveProject = function(){
        var errors = projectManager.validateAndPrepareToSave();

        if (errors.length > 0) {
            $scope.$parent.openInformationDialogClick("Errors saving the Project", errors.join('<br>'));
            return;
        }
        projectService.updateProject(projectManager.project)
            .success(function () {
                $scope.sectionForm.$setPristine();
                $scope.refreshData();

                $rootScope.$broadcast('mediaLibraryRefreshRequest');

                $scope.showView('layout-menu');
            })
            .error(function (data, status) {
                if (status == 404) {
                    $scope.$parent.openInformationDialogClick(null, "The project you are trying to access does not exist");
                }
                else if (status == 401) {
                    $scope.$parent.openInformationDialogClick(null, "Your current session expired please log in again.");
                }
                else {
                    $scope.$parent.openInformationDialogClick(null, 'An error has occurred. Please try again or contact support for assistance: \n Status: ' + status + ',\n Description: ' + data.Message);
                }
            });
    };

    $rootScope.$on(events.saveAndExportForReview, function () {
        $scope.exportForReview();
    });

    $scope.exportForReview = function () {

        if ($scope.sectionForm.$valid) {
            if ($scope.sectionForm.$pristine) {
                $rootScope.openSelectReviewContentIDsDlgClick();
            }
            else {
                $rootScope.openConfimDialogClick('CONFIRM', 'Before exporting for review, the information on this page has to be saved. Do you want to continue?', function () {
                    exportProjectForReview();
                });
            }
        }
        else {
            $scope.$parent.openInformationDialogClick("ERROR", "Before exporting for review make sure to fix all the errors below.");
        }
    };

    var exportProjectForReview = function(){
        var errors = projectManager.validateAndPrepareToSave();

        if (errors.length > 0) {
            $scope.$parent.openInformationDialogClick("Errors saving the Project", errors.join('<br>'));
            return;
        }

        projectService.updateProject(projectManager.project)
            .success(function () {
                $scope.sectionForm.$setPristine();
                $scope.refreshData();

                $rootScope.$broadcast('mediaLibraryRefreshRequest');

                $rootScope.openSelectReviewContentIDsDlgClick();
            })
            .error(function (data, status) {
                if (status == 404) {
                    $scope.$parent.openInformationDialogClick(null, "The project you are trying to access does not exist");
                }
                else if (status == 401) {
                    $scope.$parent.openInformationDialogClick(null, "Your current session expired please log in again.");
                }
                else {
                    $scope.$parent.openInformationDialogClick(null, 'An error has occurred. Please try again or contact support for assistance: \n Status: ' + status + ',\n Description: ' + data.Message);
                }
            });
    };

    $scope.$on('saveAndExportToZip', function () {
        $scope.exportToZip();
    });

    $scope.exportToZip = function () {

        if ($scope.sectionForm.$valid) {
            if ($scope.sectionForm.$pristine) {
                $scope.$parent.openSelectZipContentIDsDlgClick();
            }
            else {
                $rootScope.openConfimDialogClick('CONFIRM', 'Before exporting the package, the information on this page has to be saved. Do you want to continue?', function () {
                    var errors = projectManager.validateAndPrepareToSave();

                    if (errors.length > 0) {
                        $scope.$parent.openInformationDialogClick("Errors saving the Project", errors.join('<br>'));
                        return;
                    }

                    projectService.updateProject(projectManager.project)
                        .success(function () {
                            $scope.sectionForm.$setPristine();
                            $scope.refreshData();

                            $rootScope.$broadcast('mediaLibraryRefreshRequest');

                            $scope.$parent.openSelectZipContentIDsDlgClick();
                        })
                        .error(function (data, status) {
                            if (status == 404) {
                                $scope.$parent.openInformationDialogClick(null, "The project you are trying to access does not exist");
                            }
                            else if (status == 401) {
                                $scope.$parent.openInformationDialogClick(null, "Your current session expired please log in again.");
                            }
                            else {
                                $scope.$parent.openInformationDialogClick(null, 'An error has occurred. Please try again or contact support for assistance: \n Status: ' + status + ',\n Description: ' + data.Message);
                            }
                        });

                });
            }
        }
        else {
            $scope.$parent.openInformationDialogClick("ERROR", "Before exporting the package make sure to fix all the errors below.");
        }
    };

    $scope.init = function () {
         window.onbeforeunload = function (event) {
            if (!$scope.sectionForm.$pristine) {
                var message = 'You will lose your changes if you leave this page without saving. Are you sure you wish to continue?';
                if (typeof event == 'undefined') {
                    event = window.event;
                }
                if (event) {
                    event.returnValue = message;
                }
                return message;
            }
        }
    };

    $scope.init();

    $scope.$on('$locationChangeStart', function (event, next, current) {
        if (!$scope.sectionForm.$pristine) {
            var answer = confirm("You will lose your changes if you leave this page without saving. Are you sure you wish to continue?")
            if (!answer) {
                event.preventDefault();
            }
        }
    });

});