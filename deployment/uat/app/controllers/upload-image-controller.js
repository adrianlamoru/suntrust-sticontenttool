appControllers.controller('UploadImageController', function ($scope, $location, projectManager, CommonService, FileUploader) {
    this.$inject = ['$scope', '$location', 'projectManager', 'CommonService', 'FileUploader'];

    var FROM_LIBRARY = 'FROM_LIBRARY';
    var FROM_INPUT = 'FROM_INPUT';
    var FROM_URL = 'FROM_URL';

    $scope.formData = {};
    $scope.formData.lastSource = '';
    $scope.formData.imageFromLibraryValue = '';
    $scope.formData.uploadInputValue = '';
    $scope.formData.imageFromURLValue = '';

    $scope.mediaVM = {};

    var uploaderDlg = $scope.uploaderDlg = new FileUploader({
        url: BASE_URL + '/api/Media/Upload/' + projectManager.projectID
    });

    $scope.loadData = function () {
        CommonService.getImagesForProject($scope.getParameterID())
            .success(function (data) {
                $scope.mediaVM = data;

                $scope.tbImagesModel = new TableModel($scope.mediaVM, null, 6);
            });
    };

    $scope.$on('uploadImageDlgOpened', function (event) {
        var form = document.getElementsByName("uploadImageForm");
        if (form && form[0]) {
           // $scope.$apply(function () {
                form[0].reset();
            //});
        }
        $scope.loadData();
    });

    $scope.imageURLChange = function () {
        $scope.clearOtherEntries(FROM_URL);
    };

    $scope.setCurrentImage = function (item) {
        $scope.clearOtherEntries(FROM_LIBRARY);

        $scope.formData.imageFromLibraryValue = item.src;
    };

    $scope.isImageSelected = function (item) {
        return $scope.formData.imageFromLibraryValue == item.src;
    };

    $scope.clearOtherEntries = function (from) {
        if (from == FROM_LIBRARY) {
            //var fileInput = angular.element(uploadImageForm.fileInput);
            //fileInput.prop('value', '');

            //uploadImageForm.fileInput.value = '';

            $scope.formData.uploadInputValue = '';
            uploaderDlg.clearQueue();

            $scope.formData.imageFromURLValue = '';
        }
        else if (from == FROM_INPUT) {
            $scope.formData.imageFromLibraryValue = '';
            $scope.formData.imageFromURLValue = '';
        }
        else { //FROM_URL
            //uploadImageForm.fileInput.value = '';
            $scope.formData.imageFromLibraryValue = '';

            $scope.formData.uploadInputValue = '';
            uploaderDlg.clearQueue();
        }

        $scope.formData.lastSource = from;
    };

    $scope.cancelButtonClick = function () {
        $scope.formData = {};
        $scope.closeUploadImageDialogClick();
    }

    $scope.confirmButtonClick = function (form) {
        if ($scope.formData.lastSource == '') {
            $scope.$parent.openInformationDialogClick(null, 'You must select an image before Confirm!, or click Cancel');
        }
        else if ($scope.formData.lastSource == FROM_LIBRARY) {
            $scope.returnResults($scope.formData.imageFromLibraryValue, false);
        }
        else if ($scope.formData.lastSource == FROM_INPUT) {
            uploaderDlg.uploadAll();
        }
        else { //FROM_URL
            if ($scope.formData.imageFromURLValue == '') {
                $scope.$parent.openInformationDialogClick(null, 'You must enter an Image URL!');
            }
            else {
                $scope.returnResults($scope.formData.imageFromURLValue, true);
            }
        }
    };

    $scope.getAbsoluteURL = function (imagePath) {
        return BASE_URL + imagePath;
    };

    $scope.returnResults = function (file, external) {
        var dataToReturn = { "src": file, "isExternal": external };

        $scope.formData = {};
        $scope.confirmUploadImageDialogOKClick(dataToReturn);
    };

    $scope.isImage = function (item) {
        var type = '|' + item.type.slice(item.type.lastIndexOf('/') + 1) + '|';
        return '|jpg|png|jpeg|bmp|gif|'.indexOf(type) !== -1;
    }

    //CALLBACKs

    uploaderDlg.onAfterAddingAll = function (addedFileItems) {
        if (addedFileItems.length > 1) {
            $scope.$parent.openInformationDialogClick(null, 'Multiples files are not allowed.');
            uploaderDlg.clearQueue();
        }
        else {
            if (!$scope.isImage(addedFileItems[0].file)) {
                $scope.$parent.openInformationDialogClick(null, 'The file is not a valid image type.');
                uploaderDlg.clearQueue();
            }
            else if (hasInvalidCharacters(addedFileItems[0].file.name)) {
                $scope.$parent.openInformationDialogClick(null, 'The file name contains non-valid characters.');
                uploaderDlg.clearQueue();
            }
            else {
                $scope.formData.uploadInputValue = addedFileItems[0].file.name;

                $scope.clearOtherEntries(FROM_INPUT);
            }
        }
    };

    uploaderDlg.onErrorItem = function (fileItem, response, status, headers) {
        if (status == 400) {
            $scope.$parent.openInformationDialogClick(null, response.Message);
        }
        else {
            console.info('onErrorItem', fileItem, response, status, headers);
            $scope.uiMsg = 'Error uploading to server. Try again or select another file.';
        }
    };

    uploaderDlg.onCompleteItem = function (fileItem, response, status, headers) {
        if (uploaderDlg._isSuccessCode(status)) {
            $scope.returnResults(response[0], false);
        }
    };

    $scope.init = function () {
        $scope.loadData();
    };

    $scope.init();
});