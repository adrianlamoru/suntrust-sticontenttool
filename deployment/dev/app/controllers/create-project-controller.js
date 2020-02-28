appControllers.controller('CreateProjectController', function ($rootScope, $scope, $filter, CommonService) {
    $scope.offersModel = null;
    $scope.showEmpty = false;

    $scope.$on('projectDialog:opened', function () {
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

    $scope.$on('offerAdded', function (event, offerVM) {
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

    $scope.createProject = function () {
        $scope.$parent.openConfimDialogClick('Confirm', 'Are you sure you want to create a Project?',
            function () {
                if ($scope.offersModel.selectedItem && $scope.offersModel.selectedItem.ID > 0) {
                    var createProjectModel = new ProjectCreateEditViewModel();
                    createProjectModel.ID = $scope.offersModel.selectedItem.ID;

                    CommonService.createProject(createProjectModel, function (data, status) {
                        $rootScope.$emit('project:inserted');
                        $scope.closeProjectDialogClick();
                        if (data.ID) {
                            $scope.$parent.gotoPage('Home/OfferProjectLayout#/layout-menu/', data.ID);
                        } else {
                            $scope.$parent.openInformationDialogClick(null, 'An error has occurred, please try again or contact support for assistance!.');
                        }

                    }, $scope.manageErrors);
                } else {
                    $scope.$parent.openInformationDialogClick(null, 'You must select a valid offer!.');
                }
            });
    };

    $scope.manageErrors = function (data, status) {
        if (status == 409) {
            $scope.$parent.openInformationDialogClick(null, 'There are conflicts in your database, this ID already exists!');
        } else {
            $scope.$parent.openInformationDialogClick(null, 'An error has occurred, please try again or contact support for assistance!.');
        }
    };

    $scope.loadData = function () {
        CommonService.getAvailableOffers(function (data) {
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