appControllers.controller('CopyContentIDsController', function ($scope, $location, projectManager, projectService) {
    this.$inject = ['$scope', '$location', 'projectManager', 'projectService'];

    var model = {
        contentIDs: [],
        selectedLayout: {}
    };

    $scope.formData = model;
    $scope.showMessage = false;

    var loadData = function () {
        $scope.formData = model;
        $scope.formData.contentIDs = [];
        $scope.showMessage = false;

        $scope.selectedLayout = $scope.projectManager.selectedLayout;
        var layouts = $scope.projectManager.project.Layouts;

        for (var i = 0; i < layouts.length; i++) {
            var layout = layouts[i];
            if (layout.ID !== $scope.selectedLayout.ID) {
                var note = layout.LayoutDetail ? layout.LayoutDetail.Note : "";

                var contentID = {
                    id: layout.ID,
                    note: note,
                    selected: $scope.formData.allItemsSelected,
                    hasData: layout.Sections.length > 0 ? true : false
                };

                if (contentID.hasData) {
                    $scope.showMessage = true;
                }

                $scope.formData.contentIDs.push(contentID);
            }
        }
    };

    $scope.$on('copyContentIDsDlgOpened', function () {
        loadData();
    });

    $scope.cancelButtonClick = function () {
        $scope.formData = {};
        $scope.closeCopyContentIDsDlgClick();
    };

    $scope.selectAll = function () {
        if (!$scope.formData.contentIDs)
            return;

        var nothing = nothingIsSelected();

        $scope.formData.contentIDs.forEach(function (contentID) {
            contentID.selected = nothing;
        });
    };

    function nothingIsSelected() {
        if (!$scope.formData.contentIDs)
            return true;

        for (var i = 0; i < $scope.formData.contentIDs.length; i++) {
            var contentID = $scope.formData.contentIDs[i];
            if (contentID.selected) {
                return false;
            }
        }

        return true;
    }


    $scope.selectAllText = function () {
        return nothingIsSelected() ? "Select All" : "Deselect All";
    };

    var getSelectedContentIDs = function () {
        var selecteds = [];

        for (var i = 0; i < $scope.formData.contentIDs.length; i++) {
            var content = $scope.formData.contentIDs[i];

            if (content.selected) {
                selecteds.push(content.id);
            }
        }

        return selecteds;
    };

    $scope.confirmButtonClick = function () {
        var contentIDs = getSelectedContentIDs();

        if (contentIDs.length == 0) {
            $scope.$parent.openInformationDialogClick(null, 'You must select at least one ContentID, or click Cancel');
        }
        else {
            try {
                projectManager.copyContentIDs(getSelectedContentIDs());

                projectService.updateProject(projectManager.project)
                    .success(function () {
                        $scope.closeCopyContentIDsDlgClick();
                        $scope.refreshData();
                        $scope.$parent.openInformationDialogClick(null, "The layout copy was successful.");
                    })
                    .error(function (data, status) {
                        if (status == 404) {
                            $scope.$parent.openInformationDialogClick(null, "The project doesn't exists");
                        }
                        else {
                            $scope.$parent.openInformationDialogClick(null, 'An error has occurred, please try again or contact support for assistance!.');
                            $scope.closeCopyContentIDsDlgClick();
                        }
                    });
            }
            catch (e) {
                $scope.$parent.openInformationDialogClick(null, "The content ID(s) couldn't be copied!. " + e.message);
            }
        }
    };
});