appControllers.controller('UploadAssetController', function ($scope, $rootScope, $location, CommonService, FileUploader) {
    this.$inject = ['$scope', '$rootScope', '$location', 'CommonService', 'FileUploader'];

    var MEDIA_LIBRARY_PUBLIC_PATH = "/MediaLibrary/Public";

    $scope.formData = {};
    $scope.formData.uploadInputValue = '';

    var currentFolder = function () {
        return $rootScope.uploadAssetFolder == null ? MEDIA_LIBRARY_PUBLIC_PATH : $rootScope.uploadAssetFolder.src;
    }

    var assetUploader = $scope.assetUploader = new FileUploader({
        url: BASE_URL + '/api/Media/UploadToFolder?folder=' + encodeURIComponent(currentFolder())
    });

    $scope.$on('mediaLibraryFolderChange', function (event, newFolder) {
        assetUploader.url = BASE_URL + '/api/Media/UploadToFolder?folder=' + encodeURIComponent(currentFolder());
    });

    $scope.cancelButtonClick = function () {
        $scope.formData = {};
        $scope.closeUploadAssetDialogClick();
    }

    $scope.confirmButtonClick = function (form) {
        if (!$scope.formData.uploadInputValue == '') {
            assetUploader.uploadAll();
        }
    };

    $scope.$on('uploadAssetDlgOpened', function (event) {
        var form = document.getElementsByName("uploadAssetForm");
        if (form && form[0]) {
            // $scope.$apply(function () {
            form[0].reset();
            //});
        }
    });

    //CALLBACKs

    assetUploader.onAfterAddingFile = function (fileItem) {
        assetUploader.url = BASE_URL + '/api/Media/UploadToFolder?folder=' + encodeURIComponent(currentFolder());
    };

    assetUploader.onAfterAddingAll = function (addedFileItems) {
        if (addedFileItems.length > 1) {
            $scope.$parent.openInformationDialogClick(null, 'Multiples files are not allowed.');
            assetUploader.clearQueue();
        }
        else if (hasInvalidCharacters(addedFileItems[0].file.name)) {
            $scope.$parent.openInformationDialogClick(null, 'The file name contains non-valid characters.');
            assetUploader.clearQueue();
        }
        else {
            $scope.formData.uploadInputValue = addedFileItems[0].file.name;
        }
    };

    assetUploader.onErrorItem = function (fileItem, response, status, headers) {
        if (status == 400) {
            $scope.$parent.openInformationDialogClick(null, response.Message);
        }
        else {
            console.info('onErrorItem', fileItem, response, status, headers);
            $scope.$parent.openInformationDialogClick(null, 'Error uploading to server. Try again or select another file.');
        }
    };

    assetUploader.onCompleteItem = function (fileItem, response, status, headers) {
        if (assetUploader._isSuccessCode(status)) {
            $scope.formData.uploadInputValue = '';
            assetUploader.clearQueue();
            $scope.$parent.uploadAssetDialogOKClick(response);
        }
    };

    $scope.init = function () {
        
    }
    
    $scope.init();
});