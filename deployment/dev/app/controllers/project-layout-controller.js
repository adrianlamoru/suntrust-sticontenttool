
appControllers.controller('ProjectLayoutController', function ($scope, $rootScope, $location, projectManager, projectService) {

    $scope.currentStep = 1;
    $scope.isProjectLayoutMenuVisible = true;

    $scope.projectManager = projectManager;

    // begin: preview-dialog
    $scope.isPreviewDlgVisible = false;

    $scope.openPreviewDialogClick = function () {
        $scope.$parent.areScrollsHidden = true;
        $scope.isPreviewDlgVisible = true;
    };

    $scope.closePreviewDialogClick = function () {
        $scope.isPreviewDlgVisible = false;
        $scope.$parent.areScrollsHidden = false;
    };
    // end: preview-dialog


    // Preview Modal Dialog Start 

    //Upload Modal Dialog Start
    $scope.isUploadImageDlgVisible = false;
    $scope.uploadImageDlgOkCallback = null;

    $scope.openUploadImageDialogClick = function (callbackOK) {
        $scope.$parent.areScrollsHidden = true;
        $scope.isUploadImageDlgVisible = true;
        $scope.uploadImageDlgOkCallback = callbackOK;

        $scope.$broadcast('uploadImageDlgOpened');
    };

    $scope.closeUploadImageDialogClick = function () {
        $scope.$parent.areScrollsHidden = false;
        $scope.isUploadImageDlgVisible = false;
    };

    $scope.confirmUploadImageDialogOKClick = function (callbackData) {
        if ($scope.uploadImageDlgOkCallback) {
            $scope.closeUploadImageDialogClick();
            $scope.uploadImageDlgOkCallback(callbackData);
        }
    };
    //Upload Modal Dialog End

    // begin: copy-content-ids-dlg
    $scope.isCopyContentIDsDlgVisible = false;

    $scope.copyContentIDsDisabled = function () {
        return projectManager.project && projectManager.project.Layouts && projectManager.project.Layouts.length < 2;
    };

    $scope.openCopyContentIDsDlgClick = function () {
        if ($scope.copyContentIDsDisabled())
            return;

        $scope.$parent.areScrollsHidden = true;
        $scope.isCopyContentIDsDlgVisible = true;
        $scope.$broadcast('copyContentIDsDlgOpened');
    };

    $scope.closeCopyContentIDsDlgClick = function () {
        $scope.isCopyContentIDsDlgVisible = false;
        $scope.$parent.areScrollsHidden = false;
    };
    // end: copy-content-ids-dlg

    $scope.updateCurrentStep = function (currentStepSelected) {
        $scope.currentStep = currentStepSelected;
    };

    $scope.getSectionID = function (view) {
        if (view.indexOf('primary-banner') > -1) {
            return 1;
        } else if (view.indexOf('details-page') > -1) {
            return 2;
        } else if (view.indexOf('recommended-accounts') > -1) {
            return 3;
        } else if (view.indexOf('all-offers') > -1) {
            return 4;
        } else if (view.indexOf('splash-page') > -1) {
            return 5;
        } else if (view.indexOf('sign-off-page') > -1) {
            return 6;
        } else if (view.indexOf('credit-cards') > -1) {
            return 7;
        } else if (view.indexOf('deposits') > -1) {
            return 8;
        } else if (view.indexOf('equity') > -1) {
            return 9;
        } else if (view.indexOf('certificates') > -1) {
            return 10;
        } else if (view.indexOf('bulletin-zone') > -1) {
            return 11;
        }

        return 0;
    };

    $scope.previewUrl = function () {
        return BASE_URL + "/Preview/Show?projectID=" + projectManager.project.ID + "&layoutID=" + projectManager.selectedLayout.ID + "&sectionType=" + projectManager.selectedSection.ID;
    };

    $scope.showView = function (view) {
        $scope.$parent.showView(view + '/' + projectManager.projectID);
    };

    $scope.toggleApproval = function () {
        projectManager.toggleApproval();

        projectService.changeProjectStatus(projectManager.projectID, projectManager.isApproved())
            .success(function () {
                
            })
            .error(function (data, status) {
                projectManager.toggleApproval();

                if (status == 404) {
                    $scope.$parent.openInformationDialogClick(null, "The project doesn't exists or was previously deleted");
                } else {
                    console.log("Error", data, status);
                    $scope.$parent.openInformationDialogClick(null, 'An error has occurred, please try again or contact support for assistance!.');
                }
            });
    };

    // begin: select-content-ids-dlg
    $scope.isSelectContentIDsDlgVisible = false;

    $scope.openSelectZipContentIDsDlgClick = function () {
        $scope.$parent.areScrollsHidden = true;
        $scope.isSelectContentIDsDlgVisible = true;

        //Posibles event's parameters (dlgTitle, dlgDescription, targetAName, targetURL)
        $scope.$broadcast('selectContentIDsDlgOpened', { targetURL: $scope.urlToDownloadAsZip });
    };

    $rootScope.openSelectReviewContentIDsDlgClick = function () {
        $scope.$parent.areScrollsHidden = true;
        $scope.isSelectContentIDsDlgVisible = true;

        //Posibles event's parameters (dlgTitle, dlgDescription, targetAName, targetURL)
        $scope.$broadcast('selectContentIDsDlgOpened', {
            dlgTitle: 'Export for Review',
            targetURL: $scope.urlToDownloadAsPDF
        });
    };

    $scope.closeSelectContentIDsDlgClick = function () {
        $scope.isSelectContentIDsDlgVisible = false;
        $scope.$parent.areScrollsHidden = false;
    };
    // end: copy-content-ids-dlg

    $scope.exportToZip = function () {
        if (projectManager.selectedSection.ID == undefined) {
            $scope.openSelectZipContentIDsDlgClick();
        }
        else {
            $scope.$broadcast('saveAndExportToZip');
        }
    };

    $scope.exportForReview = function () {
        if (projectManager.selectedSection.ID == undefined) {
            $rootScope.openSelectReviewContentIDsDlgClick();
        }
        else {
            $rootScope.$broadcast(events.saveAndExportForReview);
        }
    };

    $scope.shareReadOnlyLink = function () {
        if (projectManager.isThereAnySectionEdited()) {
            var url = BASE_URL + "/Preview/Project/" + projectManager.project.Guid;
            window.open(url, '_blank');
        }
    };

    $scope.exportContents = function () {
        projectService.exportProjectContents(projectManager.projectID);
    };

    $scope.importContents = function () {
        alert('importContents');
    };
    
    $scope.getParameterID = function () {
        var parts = $location.path().split('/');

        if (parts && parts.length > 0) {
            return parts[parts.length - 1];
        }

        return 0;
    };

    $scope.init = function () {
        projectManager.projectID = $scope.getParameterID();

        if (!projectManager.projectID) {
            window.location = BASE_URL;
        }

        $scope.urlToDownloadAsZip = BASE_URL + "/Project/Export?pid=" + projectManager.projectID;
        $scope.urlToDownloadAsPDF = BASE_URL + "/PDF/Download?pid=" + projectManager.projectID;

        $scope.refreshData();
    };

    $scope.refreshData = function (successCallback, errorCallback) {
        projectService.getProjectById(projectManager.projectID)
            .success(function (project) {
                var sectionID = $scope.getSectionID($location.path());
                projectManager.setProject(project, sectionID);
                if (successCallback)
                    successCallback();
            })
            .error(function (data, status) {
                projectManager.setProject({});
                if (errorCallback)
                    errorCallback();

                console.log("Error", data, status);
                $scope.$parent.openInformationDialogClick(null, 'An error has occurred, please refresh the page or contact support for assistance!.');
            });
    };

    $rootScope.$on('offer:deleted', function (e, offerID) {
        if (projectManager.projectID == offerID) {
            window.location = BASE_URL;
        }
    });

    $rootScope.$on('offerEdited', function (e, offerID) {
        if (projectManager.projectID == offerID) {
            projectManager.selectedLayout = {};
            $scope.init();
        }
    });

    $scope.init();
});

