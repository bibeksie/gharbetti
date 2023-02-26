var app = angular.module('floorIndex', []);


app.controller('formController', ['$scope', '$filter', '$compile', '$http', '$rootScope', '$timeout', '$q', '$log', '$window',
    function ($scope, $filter, $compile, $http, $rootScope, $timeout, $q, $log, $window) {
        $scope.FloorList = [];
        $scope.Floor = {
            Id: 0,
            Name: "",
            IsActive: true,
        }

        $scope.init = function (floorList) {

            $scope.FloorList = floorList;
        }

        $scope.onClickAdd = function () {

            if ($scope.Floor.Id == "") {
                data = $scope.Floor;
                $http.post("/api/Floor/Add", data).then(function (responsedata) {
                    debugger;
                    console.log(responsedata.data);
                    if (responsedata.data.Status) {
                        location.reload();
                    }
                    else {
                        alert("Error Occured");
                    }
                });
            }
            else {
                data = $scope.Floor
                $http.post('/api/Floor/Edit', data).then(function (responsedata) {
                    debugger;
                    console.log(responsedata.data);
                    if (responsedata.data.Status) {
                        location.reload();
                    }
                    else {
                        alert("Error Occured");
                    }
                });

            }
        }

        $scope.onClickAddModal = function () {
            $scope.Floor = {
                Id: 0,
                Name: "",
                IsActive: true,
            }

            $('#addModal').modal('show');
        }

        $scope.onClickCloseModal = function () {
            $('#addModal').modal('hide');
        }

        $scope.ShowEdit = function (id) {

            $http({
                url: "/api/Floor/Edit",
                method: "GET",
                params: { id: id }
            }).then(function (responsedata) {
                debugger;
                console.log(responsedata.data);
                if (responsedata.data) {
                    var data = responsedata.data.Data;
                    $scope.Floor.Id = data.Id;
                    $scope.Floor.Name = data.Name;

                    $('#addModal').modal('show');
                }
                else {
                    alert(responsedata.data.Message);
                }
            });
          
        }


        $scope.Delete = function (id) {

            $http({
                url: "/api/Floor/Delete",
                method: "GET",
                params: { id: id }
            }).then(function (responsedata) {
                debugger;
                console.log(responsedata.data);
                if (responsedata.data) {
                    alert(responsedata.data.Message);
                    location.reload();
                   }
                else {
                    alert(responsedata.data.Message);
                }
            });

        }
    }]);









