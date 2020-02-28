appControllers.controller('SearchResultsController', function ($rootScope, $scope, $filter, $location, $route, CommonService) {
    $scope.searchResultsVM = [];
    $scope.mediaAssetsVM = [];

    $scope.tbAllProjectsModel = null;
    $scope.tbImagesModel = null;

    $scope.isSearhEmpty = true;
    $scope.isProjectResultEmpty = true;
    $scope.isMediaAssetResultEmpty = true;
    $scope.isDownloadButtonDisabled = true;

    $scope.sortBy = function (model, value) {
        $scope.tbAllProjectsModel.sortBy(value);
    };

    $scope.getAbsoluteURL = function (imagePath) {
        return BASE_URL + imagePath;
    };

    $scope.$on('searchChanged', function (event, text) {
        $scope.loadData(text);
    });

    $scope.loadData = function (text) {
        $scope.$parent.searchString = text == undefined ? $location.search().f : text;
        $scope.isSearhEmpty = $scope.$parent.searchString == undefined || $scope.$parent.searchString == '';

        if ($scope.isSearhEmpty) {
            return;
        }

        CommonService.searchInProjects($scope.$parent.searchString)
            .success(function (data) {
                $scope.searchResultsVM = data.Projects;
                $scope.mediaAssetsVM = data.Assets;

                $scope.isProjectResultEmpty = $scope.searchResultsVM.length == 0;
                $scope.isMediaAssetResultEmpty = $scope.mediaAssetsVM.length == 0;

                $scope.tbAllProjectsModel = new TableModel($scope.searchResultsVM, $filter('orderBy'));
                $scope.tbAllProjectsModel.sortBy('DateCreated', true);
                
                $scope.tbImagesModel = new TableModel($scope.mediaAssetsVM, null, 6);

            })
            .error(function (data, status, header, config) {
                console.log("Error", data, status);
                $scope.$parent.openInformationDialogClick(false, 'Unexpected error during the Search.', 'Continue');
            });
    };

    $scope.updateDownloadButtonStatus = function () {
        $scope.isDownloadButtonDisabled = true;

        var currentPage = $scope.tbImagesModel.pagedItems[$scope.tbImagesModel.currentPage];

        if (currentPage != null) {
            for (var i = 0; i < currentPage.length; i++) {
                if (currentPage[i].isSelected) {
                    $scope.isDownloadButtonDisabled = false;
                    return;
                }
            }
        }
    };

    $scope.getImagePreview = function (fileItem) {
        return getImagePreview(fileItem);
    }

    var getSelectedItems = function () {
        var selectedItems = [];
        var currentPage = $scope.tbImagesModel.pagedItems[$scope.tbImagesModel.currentPage];

        for (var i = 0; i < currentPage.length; i++) {
            if (currentPage[i].isFile && currentPage[i].isSelected) {
                selectedItems.push(currentPage[i]);
            }
        }

        return selectedItems;
    }

    $scope.resetSelectedValues = function () {
        $scope.isDownloadButtonDisabled = true;

        var currentPage = $scope.tbImagesModel.pagedItems[$scope.tbImagesModel.currentPage];

        if (currentPage != null) {
            for (var i = 0; i < currentPage.length; i++) {
                if (currentPage[i].isFile) {
                    currentPage[i].isSelected = false;
                }
            }
        }
    }

    $scope.downloadSelected = function () {
        var selectedItems = getSelectedItems();

        if (selectedItems.length > 0) {
            $rootScope.openConfimDialogClick('Are you sure?', 'Are you sure you want to download the Assets?',
                function () {
                    var form = document.createElement("form");
                    form.setAttribute("action", BASE_URL + '/Media/DownloadBulk');
                    form.setAttribute("method", "post");

                    var hiddenEle1 = document.createElement("input");
                    hiddenEle1.setAttribute("type", "hidden");
                    hiddenEle1.setAttribute("name", "assetList");
                    hiddenEle1.setAttribute("value", JSON.stringify(selectedItems));

                    form.appendChild(hiddenEle1);

                    form.submit();
                },
                'Continue');
        }
        else {
            $scope.$parent.openInformationDialogClick(false, 'They are not selected assets.', 'Continue');
        }
    }

    $scope.init = function () {
        $scope.loadData();
    };

    $scope.init();
});