appControllers.controller('ProjectsSelectionController', function ($scope) {
    this.$inject = ['$scope'];

    $scope.projectSelectionDlgData = {
        title: "Dialog title",
        description: "Dialog description",
        projects: null,
        onYesClick: null,
        onCancelClick: null
    };

    $scope.$on('projects-selection-dlg:opened', function (event, eventData) {
        $scope.projectSelectionDlgData.title = eventData.title;
        $scope.projectSelectionDlgData.description = eventData.description;
        $scope.projectSelectionDlgData.projects = eventData.data;
        $scope.projectSelectionDlgData.onYesClick = eventData.onYesClick;
        $scope.projectSelectionDlgData.onCancelClick = eventData.onCancelClick;
    });

    $scope.yesClick = function () {
        if ($scope.projectSelectionDlgData.onYesClick) {
            $scope.projectSelectionDlgData.onYesClick();
        }
    };

    $scope.cancelClick = function () {
        if ($scope.projectSelectionDlgData.onCancelClick) {
            $scope.projectSelectionDlgData.onCancelClick();
        }
    }
});