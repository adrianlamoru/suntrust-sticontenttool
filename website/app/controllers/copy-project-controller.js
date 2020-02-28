appControllers.controller('CopyProjectController', function ($scope, $rootScope, $filter, $location, projectManager, projectService, DashboardService, CommonService) {
    this.$inject = ['$scope', '$rootScope', '$filter', '$location', 'projectManager', 'projectService', 'DashboardService'];

    var model = {
        contentIDs: [],
        selectedLayout: {}
    };

    $scope.formData = model;
    $scope.showMessage = false;
    $scope.copyButtonDisabled = true;
    $scope.activeProjects = [];
    $scope.sourceContentIDs = [];
    $scope.selectedProjectToCopy = null;
    $scope.targetOfferId = -1;
    $scope.targetOffer = null;
    $scope.copyObjects = [];
    $scope.showCopyMessage = false;
    
    var loadData = function () {
        $scope.formData = model;
        $scope.formData.contentIDs = [];
        $scope.showMessage = false;

        DashboardService.getActiveProjectsByOfferType(CURRENT_USER_TYPE_ID)
            .success(function (projects) {
                $scope.activeProjects = $filter('orderBy')(projects, 'OfferName');
            })
            .error(function (data, status) {
                console.log("Error", data, status);
            });

        if ($scope.targetOfferId == -1) {
            $scope.selectedLayout = $scope.projectManager.selectedLayout;
            var layouts = $scope.projectManager.project.Layouts;

            for (var i = 0; i < layouts.length; i++) {
                var layout = layouts[i];
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
        } else {
           
        }       
    };

    $scope.$on('copyProjectDlgOpened', function (event, eventData) {
        $scope.selectedProjectToCopy = null;
        if (eventData) {
            if (eventData.dlgTitle) {
                $scope.dlgTitle = eventData.dlgTitle;
            }
            if (eventData.dlgDescription) {
                $scope.dlgDescription = eventData.dlgDescription;
            }
            if (eventData.targetOfferId) {
                $scope.targetOfferId = eventData.targetOfferId;
            } else {
                $scope.targetOfferId = -1;
            }
            if (eventData.targetOffer) {
                $scope.targetOffer = eventData.targetOffer;
                init($scope.targetOffer.ID);
            }
            if (eventData.onCancelClick) {
                $scope.onCancelClick = eventData.onCancelClick;
            }
        }
        loadData();
    });

    $scope.cancelButtonClick = function () {
        if ($scope.onCancelClick) {
            $scope.onCancelClick();
        }
    };

    $scope.$watch('selectedProjectToCopy', function (newValue, oldValue) {
        $scope.sourceContentIDs = [];        
        if ($scope.selectedProjectToCopy) {
            $scope.copyButtonDisabled = false;
            var tempContentIDs = $scope.selectedProjectToCopy.ContentIDs.split(',');

            angular.forEach(tempContentIDs, function (contentID) {
                if (contentID.trim() !== '') {
                    $scope.sourceContentIDs.push(contentID);
                }
            });

            $scope.selected = $scope.sourceContentIDs[0];
            if (newValue != oldValue) {
                $scope.copyObjects = [];
                angular.forEach($scope.formData.contentIDs, function (contentID) {
                    if (contentID.id.trim() !== '') {
                        $scope.copyObjects.push({
                            source: {
                                id: $scope.sourceContentIDs[0]
                            },
                            target: {
                                id: contentID.id
                            }
                        });
                    }
                });
            }
        } else {
            $scope.copyButtonDisabled = true;
        }     

       
    });

    $scope.updateSourceContentID = function (copyObjectIdx, test) {
        $scope.copyObjects[copyObjectIdx].source.id = test.selected;
        $scope.copyObjects[copyObjectIdx].target.id = this.item.id;
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

    var init = function (offerId) {
        if ($scope.targetOfferId != -1) {
            CommonService.getOfferForEdit(offerId,
                function (data) {
                    var contentIDsArray = data.ContentIDs.split(",");
                    angular.forEach(contentIDsArray, function (contentID) {
                        if (contentID.trim() !== '') {
                            var contentIDObj = {
                                id: contentID,
                                note: "",
                                selected: false,
                                hasData: false
                            };
                            $scope.formData.contentIDs.push(contentIDObj);
                        }
                    });
                },
                function (data, status) {
                    console.log("Error", data, status);
                });
        }
       
    };
    $scope.reloadProject = function () {
        if ($scope.targetOfferId == -1) {
            location.reload();
        } else {
            $scope.cancelButtonClick();
            $scope.showCopyMessage = false;
            $rootScope.$broadcast('offerEdited', $scope.selectedProjectToCopy.OfferID);
        }
    }

    $scope.copyButtonClick = function () {
                
        var tempSourceContentIDs = $scope.selectedProjectToCopy.ContentIDs.split(',');
        var contentIDsToCopy = [];
        angular.forEach(tempSourceContentIDs, function (contentID) {
            if (contentID.trim() !== '') {
                contentIDsToCopy.push(contentID);
            }
        });
        var sourceContentIDs = [];
        var targetContentIDs = [];
        
        angular.forEach($scope.copyObjects, function (copyObject) {
            sourceContentIDs.push(copyObject.source.id);
            targetContentIDs.push(copyObject.target.id);
        });
        CommonService.copyFromOffer({
            sourceProjectID: $scope.selectedProjectToCopy.OfferID,
            targetProjectID: $scope.targetOffer.ID,
            sourceContentIDs: sourceContentIDs,
            targetContentIDs: targetContentIDs
        },
        function (data, status) {
            $scope.showCopyMessage = true;
            $scope.dlgTitle = "Information";
            if ($scope.targetOfferId == -1) {
                $scope.dlgDescription = "The project content copy was successful.";
            } else {
                $scope.dlgDescription = "The offer content copy was successful.";
            }
            
        },
        function (data, status) {
            $scope.showCopyMessage = true;
            console.log("data", data);
            $scope.dlgTitle = "Information";
            $scope.dlgDescription = "An error occur during the copy proccess.";
        });
    };
});