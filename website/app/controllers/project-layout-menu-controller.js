
appControllers.controller('ProjectLayoutMenuController', function ($scope, projectManager) {
   
    $scope.showSection = function (view) {
        projectManager.selectedSection = {};
        projectManager.setSelectedSection($scope.getSectionID(view));
        $scope.$parent.showView(view);
    };

    $scope.activeSections = [];

    $scope.opened = function (id) {
        return $scope.activeSections.indexOf(id) > -1;
    };

    $scope.toggle = function (id) {
        var index = $scope.activeSections.indexOf(id);
        if (index > -1) {
            $scope.activeSections.splice(index, 1);
        } else {
            $scope.activeSections.push(id);
        }
    };

    $scope.isWebProject = function (offerTypeId) {
        return offerTypeId == 0;
    };
});