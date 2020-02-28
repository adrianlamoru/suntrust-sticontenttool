appControllers.controller('SelectContentIDsController', function ($scope, $location, projectManager, projectService) {
    this.$inject = ['$scope', '$location', 'projectManager', 'projectService'];

    $scope.formData = {
        contentIDs: []
    };

    $scope.dlgTitle = 'Export for Web Team';
    $scope.dlgDescription = 'Select the Content ID(s) to export.';
    
    $scope.targetURL = $scope.urlToDownloadAsZip;
    $scope.showMessage = false;

    var loadData = function () {
        $scope.formData = {
            contentIDs: []
        };
        $scope.showMessage = false;

        var layouts = $scope.projectManager.project.Layouts;

        for (var i = 0; i < layouts.length; i++) {
            var layout = layouts[i];

            var note = layout.LayoutDetail ? layout.LayoutDetail.Note : "";
            var contentID = {
                id: layout.ID,
                note: note,
                selected: false,
                hasData: layout.Sections.length > 0 ? true : false
            };

            if (!contentID.hasData) {
                $scope.showMessage = true;
            }

            $scope.formData.contentIDs.push(contentID);
        }
    };

    $scope.$on('selectContentIDsDlgOpened', function (event, eventData) {
        if (eventData) {
            if (eventData.dlgTitle) {
                $scope.dlgTitle = eventData.dlgTitle;
            }

            if (eventData.dlgDescription) {
                $scope.dlgDescription = eventData.dlgDescription;
            }

            if (eventData.targetURL) {
                $scope.targetURL = eventData.targetURL;
            }
        }

        loadData();
    });

    $scope.confirmButtonClick = function () {
        var contentIDs = getSelectedContentIDs();

        if (contentIDs.length == 0) {
            $scope.$parent.openInformationDialogClick(null, 'You must select at least one ContentID, or click Cancel');
        }
        else {
            var cids = '';

            for (var i = 0; i < contentIDs.length; i++) {
                if (cids) {
                    cids += ',';
                }

                cids += contentIDs[i];
            }

            //var a = document.getElementById($scope.targetAName);
            //a.href = $scope.targetURL + cids
            //a.click();

            post($scope.targetURL, { "cids": cids });

            $scope.closeSelectContentIDsDlgClick();
        }
    };

    $scope.cancelButtonClick = function () {
        $scope.formData = {};
        $scope.closeSelectContentIDsDlgClick();
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


});