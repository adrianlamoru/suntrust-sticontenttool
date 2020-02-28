appControllers.controller('SharedMediaLibraryController', function ($scope, $rootScope, $location, $http, CommonService, FileUploader) {
    this.$inject = ['$scope', '$rootScope', '$location', '$http', 'CommonService', 'FileUploader'];

    var FILTER_ALL_VALUE = 'ALL';
    var FILTER_NONE_VALUE = 'NONE';
    var CURRENT_FOLDER_PARAM_NAME = 'f';
    var CURRENT_FOLDER_TYPE_PARAM_NAME = 'c';
    
    $scope.formData = {};
    $scope.formData.selectedFolder = null;
    $scope.formData.folderTitle = 'Existing Assets';
    $scope.formData.isRootFolderSelected = true;
    $scope.formData.selectAllValue
    $scope.formData.isBulkActionDisabled = false;

    $scope.formData.selectedAssetTypeFilter = FILTER_ALL_VALUE;
    $scope.formData.selectedFileTypeFilter = FILTER_ALL_VALUE;
    $scope.formData.bulkActionSelected = FILTER_NONE_VALUE;

    $scope.formData.uploadInputValue = '';

    $scope.mediaAssetsVM = {};
    $scope.tbImagesModel = {};

    var createJsonFolder = function (folderID, isCustomFolder) {
        return {
            'name': folderID,
            'title': folderID + (isCustomFolder ? '' : '-Project'),
            'src': '/MediaLibrary/' + (isCustomFolder ? 'Public/' : 'Project/') + folderID,
            'isCustomFolder': isCustomFolder,
            'isProjectFolder': !isCustomFolder
        };
    }

    $scope.setSelectedFolder = function (newFolder) {
        if ($scope.formData.selectedFolder != newFolder) {
            $scope.formData.selectedFolder = newFolder;
            $scope.formData.folderTitle = newFolder == null ? 'Existing Assests' : newFolder.title;
            $scope.formData.isRootFolderSelected = $scope.formData.selectedFolder == null;

            $rootScope.uploadAssetFolder = newFolder;
            $rootScope.$broadcast('mediaLibraryFolderChange', newFolder);

            if (isSharedMediaLibraryPage($location.absUrl())){
                if (newFolder != null) {
                    $location.search(CURRENT_FOLDER_PARAM_NAME, newFolder.name);
                    $location.search(CURRENT_FOLDER_TYPE_PARAM_NAME, newFolder.isCustomFolder);
                }
                else {
                    if ($scope.getProjectID() != -1) {
                        $location.search(CURRENT_FOLDER_PARAM_NAME, $scope.getProjectID());
                        $location.search(CURRENT_FOLDER_TYPE_PARAM_NAME, false);
                    }
                    else {
                        $location.search(CURRENT_FOLDER_PARAM_NAME, '0');
                        $location.search(CURRENT_FOLDER_TYPE_PARAM_NAME, false);
                    }
                }
            }

            $scope.loadData();
        }
    }

    var isParamNullOrEqualToZero = function (param, url) {
        //TODO: Make the same, but creating some URL object and getting its properties
        //var parser = document.createElement('a');
        //parser.href = "http://example.com:3000/pathname/?search=test#hash";

        //parser.protocol; // => "http:"
        //parser.hostname; // => "example.com"
        //parser.port;     // => "3000"
        //parser.pathname; // => "/pathname/"
        //parser.search;   // => "?search=test"
        //parser.hash;     // => "#hash"
        //parser.host;     // => "example.com:3000"

        return url.indexOf('?' + param + '=') == -1 || url.indexOf('?' + param + '=0') > -1; 
    }

    $scope.$on('$locationChangeStart', function (event, next, current) {
        if (isSharedMediaLibraryPage(next)) {
            if (isParamNullOrEqualToZero(CURRENT_FOLDER_PARAM_NAME, next) && !$scope.formData.isRootFolderSelected) {

                $scope.setSelectedFolder(null);
            }
            else {
                //In a future: Navigate to the folder ID pointed by the param
                //Get the param values
                //var folderID = getParamValueFromUrlString(CURRENT_FOLDER_PARAM_NAME, next);
                //var folderID = getParamValueFromUrlString(CURRENT_FOLDER_TYPE_PARAM_NAME, next);
                //$scope.setSelectedFolder(createJsonFolder(folderID, isCustomFolder));
            }
        }
    });


    var isSharedMediaLibraryPage = function (path) {
        return path.indexOf('/MediaLibrary/Index') != -1
    }

    $scope.applyFilter = function () {
        var filteredList = []; 

        if (($scope.formData.isRootFolderSelected && $scope.formData.selectedAssetTypeFilter == FILTER_ALL_VALUE) ||
                (!$scope.formData.isRootFolderSelected && $scope.formData.selectedFileTypeFilter == FILTER_ALL_VALUE)) {
            filteredList = $scope.mediaAssetsVM;
        }
        else {
            if ($scope.formData.isRootFolderSelected) {
                for (var i = 0; i < $scope.mediaAssetsVM.length; i++) {
                    if ($scope.mediaAssetsVM[i].type == $scope.formData.selectedAssetTypeFilter) {
                        filteredList.push($scope.mediaAssetsVM[i]);
                    }
                }
            }
            else {
                for (var i = 0; i < $scope.mediaAssetsVM.length; i++) {
                    if ($scope.mediaAssetsVM[i].subtype == $scope.formData.selectedFileTypeFilter) {
                        filteredList.push($scope.mediaAssetsVM[i]);
                    }
                }
            }
        }

        $scope.tbImagesModel = new TableModel(filteredList, null, 20);
        $scope.updateApplyBulkStatus();
    };

    $scope.$on('mediaLibraryRefreshRequest', function (event) {
        $scope.loadData();
    });

    $scope.loadData = function () {
        if ($scope.formData.isRootFolderSelected) {
            var json = { "filter": "ALL" };

            CommonService.getAllAssets(json)
                .success(function (data) {
                    $scope.mediaAssetsVM = data;
                    $scope.formData.selectedAssetTypeFilter = FILTER_ALL_VALUE;

                    $scope.applyFilter();
                })
                .error(function (data, status) {
                    console.log("Error", data, status);
                });
        }
        else {
            var json = { "path": $scope.formData.selectedFolder.src, "filter": "ALL" };

            CommonService.getAssetsInFolder(json)
                .success(function (data) {
                    $scope.mediaAssetsVM = data;
                    $scope.formData.selectedAssetTypeFilter = FILTER_ALL_VALUE;

                    $scope.applyFilter();
                })
                .error(function (data, status) {
                    console.log("Error", data, status);
                });
        }
    };

    $scope.getImagePreview = function (fileItem) {
        return getImagePreview(fileItem);
    }

    $scope.isImage = function (item) {
        var type = '|' + item.type.slice(item.type.lastIndexOf('/') + 1) + '|';
        return '|jpg|png|jpeg|bmp|gif|'.indexOf(type) !== -1;
    };

    $scope.applyBulkAction = function () {
        if ($scope.formData.bulkActionSelected == 'DOWNLOAD') {
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
        } else if ($scope.formData.bulkActionSelected == 'DELETE') {
            var selectedItems = getSelectedItems();

            if (selectedItems.length > 0) {
                $rootScope.openConfimDialogClick('Are you sure?', 'Deleting an asset will permanently remove it from the system. Any selected assets that are currently in use will disappear from the associated layout(s). Are you sure you wish to continue?',
                    function () {
                        CommonService.deleteBulkMediaAssets(selectedItems)
                            .success(function (data) {
                                var currentPage = $scope.tbImagesModel.currentPage;

                                removeItems($scope.mediaAssetsVM, data);
                                $scope.applyFilter();

                                if ($scope.tbImagesModel.pagedItemsLength > currentPage) {
                                    $scope.tbImagesModel.currentPage = currentPage;
                                }

                                if (data.length == selectedItems.length) {
                                    $scope.$parent.openInformationDialogClick(false, 'The assets were successfully deleted.', 'Continue');
                                }
                                else {
                                    $scope.$parent.openInformationDialogClick(false, 'The assets were not completely deleted. Selected to delete: ' + selectedItems.length + ', Deleted: ' + data.length, 'Continue');
                                }
                            })
                            .error(function (data, status, header, config) {
                                console.log("Error", data, status);
                                $scope.$parent.openInformationDialogClick(false, 'Unexpected error. The assets were not completely deleted.', 'Continue');
                            });
                    },
                    'Continue');
            }
            else {
                $scope.$parent.openInformationDialogClick(false, 'They are not selected assets.', 'Continue');
            }
        }
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

    var removeItem = function (mediaAssetsVM, item) {
        for (var i = 0; i < mediaAssetsVM.length; i++) {
            if (mediaAssetsVM[i].src == item.src) {
                mediaAssetsVM.splice(i, 1);
            }
        }
    }

    var removeItems = function (mediaAssetsVM, list) {
        if (list != null) {
            for (var i = 0; i < mediaAssetsVM.length; i++) {
                for (var j = 0; j < list.length; j++) {
                    if (mediaAssetsVM[i].src == list[j].src) {
                        mediaAssetsVM.splice(i, 1);
                    }
                }
            }
        }
    }

    $scope.resetSelectAllValue = function () {
        $scope.formData.selectAllValue = false;
        $scope.setSelectedValueToFiles(false);
        $scope.updateApplyBulkStatus();
    }

    $scope.setSelectedValueToFiles = function (value) {
        var currentPage = $scope.tbImagesModel.pagedItems[$scope.tbImagesModel.currentPage];

        if (currentPage != null) {
            for (var i = 0; i < currentPage.length; i++) {
                if (currentPage[i].isFile) {
                    currentPage[i].isSelected = value != undefined ? value : !$scope.formData.selectAllValue;
                }
            }
        }
    }

    $scope.updateApplyBulkStatus = function (value) {
        var currentPage = $scope.tbImagesModel.pagedItems[$scope.tbImagesModel.currentPage];
        $scope.formData.isBulkActionDisabled = true;

        if (currentPage != null) {
            for (var i = 0; i < currentPage.length; i++) {
                if (currentPage[i].isFile) {
                    $scope.formData.isBulkActionDisabled = false;
                    $scope.formData.bulkActionSelected = 'NONE';
                    return;
                }
            }
        }
    }

    $scope.deleteFolder = function (item, event) {
        event.stopPropagation();

        if ($scope.formData.isRootFolderSelected) {
            $rootScope.openConfimDialogClick('Are you sure?', 'Deleting the folder "' + item.title + '" also will permanently delete all contained assets. Are you sure you wish to continue?',
                function () {
                    var json = { "path": item.name };

                    CommonService.deleteMediaFolder(json)
                        .success(function (data) {

                            removeItem($scope.mediaAssetsVM, item);
                            $scope.applyFilter();

                            $scope.$parent.openInformationDialogClick(false, 'The folder "' + item.title + '" was successfully deleted.', 'Continue');
                        })
                        .error(function (data, status, header, config) {
                            console.log("Error", data, status);
                            $scope.$parent.openInformationDialogClick(false, 'Unexpected error. The folder "' + inputText + '" was not deleted.', 'Continue');
                        });
                },
                'Continue');
        }
    }

    $scope.deleteFile = function (item, event) {
        event.stopPropagation();

        $rootScope.openConfimDialogClick('Are you sure?', 'Are you sure you wish to delete the item "' + item.title + '?',
            function () {
                //var json = { "path": $scope.getProjectID() != -1 ? $scope.getProjectID() + '/' + item.name : item.name };
                var json = { "path": item.src };

                CommonService.deleteMediaFile(json)
                    .success(function (data) {

                        //romoveFile($scope.mediaAssetsVM, item);
                        removeItem($scope.mediaAssetsVM, item);
                        $scope.applyFilter();

                        $scope.$parent.openInformationDialogClick(false, 'The file "' + item.title + '" was successfully deleted.', 'Continue');
                    })
                    .error(function (data, status, header, config) {
                        console.log("Error", data, status);
                        $scope.$parent.openInformationDialogClick(false, 'Unexpected error. The file "' + inputText + '" was not deleted.', 'Continue');
                    });
            },
            'Continue');
    }

    $scope.openCreateNewFolderDialog = function () {
        if ($scope.formData.isRootFolderSelected) {
            $scope.$parent.openInputDialogClick('Create Folder', 'Enter Folder Name',
                function (inputText) {
                    var json = { "path": inputText };

                    CommonService.createMediaFolder(json)
                        .success(function (data) {
                            $scope.mediaAssetsVM.unshift(data);
                            $scope.applyFilter();

                            $scope.$parent.openInformationDialogClick(false, 'The folder "' + inputText + '" was successfully created.', 'Continue');
                        })
                        .error(function (data, status, header, config) {
                            console.log("Error", data, status);
                            $scope.$parent.openInformationDialogClick(false, 'Unexpected error. The folder "' + inputText + '" was not created.', 'Continue');
                        });
                },
                'Confirm');
        }
    };

    $scope.openAddMediaDialog = function () {
        $scope.$parent.openUploadAssetDialogClick('Upload Asset​', 'Choose an asset to upload from your desktop.',
            function (assets) {
                //Insert into list of assets, refresh the filter
                $scope.mediaAssetsVM.push(assets[0]);
                $scope.applyFilter();

                $scope.$parent.openInformationDialogClick(false, 'The file "' + assets[0].name + '" was successfully uploaded.', 'Continue');
            },
            'Continue', 
            $scope.formData.selectedFolder);
    }

    $scope.getProjectID = function () {
        if ($location.absUrl().indexOf('OfferProjectLayout#/layout-menu') > 0) {
            var parts = $location.path().split('/');

            if (parts && parts.length > 0) {
                return parts[parts.length - 1];
            }
        }

        return -1;
    };

    $scope.init = function () {
        var projectId = $scope.getProjectID();

        if ( projectId > 0) {
            $scope.setSelectedFolder(createJsonFolder(projectId, false));
        }
        else {
            $scope.loadData();
        }
    }
    
    $scope.init();
});