appControllers.controller('LayoutDetailsController', function ($scope, projectService, projectManager) {
    $scope.projectManager = projectManager;
    // $scope.layoutDetail = projectManager.selectedLayout.LayoutDetail;
    var editing = false;

    $scope.ViewMode = ViewMode;

    $scope.layoutDetail = function () {
        return $scope.projectManager.selectedLayout && $scope.projectManager.selectedLayout.LayoutDetail ? $scope.projectManager.selectedLayout.LayoutDetail : {};
    };

    $scope.viewMode = function () {
        if (!$scope.layoutDetail().ID && !editing) {
            return ViewMode.CREATE
        } else if (editing) {
            return ViewMode.EDIT;
        }

        return ViewMode.VIEW;
    };

    $scope.edit = function () {
        editing = true;
    };

    $scope.save = function () {
        var layoutDetail = $scope.layoutDetail();
        if (layoutDetail.ID && !layoutDetail.Note) {
            $scope.delete();
        } else if (layoutDetail.ID) {
            // edit
            projectService.updateLayoutDetail(layoutDetail.ID, layoutDetail)
                .then(function (result) {

                }, function (data, status) {
                    console.log("Error", data, status);
                    $scope.$parent.openInformationDialogClick(null, 'An error has occurred, please try again or contact support for assistance!.');
                });
            editing = false;
        } else {
            // add
            if (layoutDetail.Note) {
                projectService.addLayoutDetail(layoutDetail)
                    .then(function (result) {
                        $scope.projectManager.selectedLayout.LayoutDetail = result.data;
                    }, function (data, status) {
                        console.log("Error", data, status);
                        $scope.$parent.openInformationDialogClick(null, 'An error has occurred, please try again or contact support for assistance!.');
                    });
            }
            editing = false;
        }
    };

    $scope.delete = function () {
        var layoutDetail = $scope.layoutDetail();
        if (layoutDetail.ID) {
            projectService.deleteLayoutDetail(layoutDetail.ID)
                .then(function (result) {
                    layoutDetail.ID = 0;
                    layoutDetail.Note = "";
                }, function (data, status) {
                    console.log("Error", data, status);
                    $scope.$parent.openInformationDialogClick(null, 'An error has occurred, please try again or contact support for assistance!.');
                });
        }
        editing = false;
    }
});