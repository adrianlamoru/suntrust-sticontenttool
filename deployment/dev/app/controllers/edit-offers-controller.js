appControllers.controller('EditOffersController', function ($rootScope, $scope, $filter, CommonService, projectService) {
    $scope.offersModel = null;
    $scope.showEmpty = false;

    $scope.$on('editOffersDialog:opened', function () {
        $scope.init();
    });

    $scope.setSelectedOffer = function (id) {
        if (id <= 0)
            return;

        if ($scope.offersModel.selectedItem && $scope.offersModel.selectedItem.ID === id)
            return;

        var array = $scope.offersModel.pagedItems[$scope.offersModel.currentPage];
        if (array && array.length > 0 && id > 0) {
            for (var i = 0; i < array.length; i++) {
                if (array[i].ID == id) {
                    $scope.offersModel.setSelectedItem(array[i]);
                    break;
                }
            }
        }
    };

    $scope.$on('offerAdded', function () {
        $scope.init();
    });

    $scope.$on('offerEdited', function () {
        $scope.init();
    });

    $scope.isAdmin = function () {
        if (CURRENT_ROL === "Administrator") {
            return true;
        }

        return false;
    }

    $scope.isSuperAdmin = function () {
        if (CURRENT_ROL === "Super Admin") {
            return true;
        }

        return false;
    }

    $scope.textAsHtml = function (text) {
        if (text)
            return text.replace(/(?:\r\n|\r|\n)/g, '<br />');
        return '';
    };

    var deleteSelectedOffer = function() {
        if ($scope.offersModel.selectedItem && $scope.offersModel.selectedItem.ID > 0) {
            CommonService.deleteOffer($scope.offersModel.selectedItem.ID)
                .success(function () {
                    $rootScope.$emit('offer:deleted', $scope.offersModel.selectedItem.ID);
                    $scope.init();
                })
                .error(function (data, status) {
                    console.log("Error", data, status);
                    $scope.$parent.openInformationDialogClick("Error", 'An error has occurred, please refresh the page or contact support for assistance!');
                });
        } else {
            $scope.$parent.openInformationDialogClick(null, 'You must select a valid offer!.');
        }
    };

    $scope.deleteOffer = function () {
        projectService.getProjectById($scope.offersModel.selectedItem.ID)
            .success(function (project) {
                $rootScope.openConfimDialogClick('Confirm', 'The current offer you are trying to delete has a project associated with it. If you delete the offer, the project will also be deleted. Are you sure you want to continue?', deleteSelectedOffer);
            })
            .error(function (data, status) {
                if (status == 404) {
                    $rootScope.openConfimDialogClick('Confirm', 'Are you sure you want to delete this Offer?', deleteSelectedOffer);
                } else {
                    console.log("Error", data, status);
                    $scope.$parent.openInformationDialogClick(null, 'An error has occurred, please refresh the page or contact support for assistance!.');
                }
            });
    };

    $scope.editOffer = function () {
        $scope.openEditOfferDialogClick($scope.offersModel.selectedItem.ID);
    };

    $scope.manageErrors = function (data, status) {
        if (status == 409) {
            $scope.$parent.openInformationDialogClick(null, 'There are conflicts in your database, this ID already exists!');
        } else {
            $scope.$parent.openInformationDialogClick(null, 'An error has occurred, please try again or contact support for assistance!.');
        }
    };

    $scope.loadData = function () {
        CommonService.getOffers(function (data) {
            $scope.showEmpty = false;
            $scope.offersModel = new TableModel(data, $filter('orderBy'));

            if ($scope.offersModel.pagedItems.length > 0) {
                $scope.offersModel.sortBy('UpdatedDate', true);
                $scope.offersModel.selectedItem = null;
                $scope.showEmpty = false;
            } else {
                $scope.showEmpty = true;
            }
        });
    };

    $scope.init = function () {
        $scope.offersModel = null;
        $scope.selectedOffer = null;
        $scope.loadData();
    };
});