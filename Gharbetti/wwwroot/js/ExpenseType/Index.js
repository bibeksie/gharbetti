var app = angular.module('expenseTypeIndex', []);


app.controller('formController', ['$scope', '$filter', '$compile', '$http', '$rootScope', '$timeout', '$q', '$log', '$window',
    function ($scope, $filter, $compile, $http, $rootScope, $timeout, $q, $log, $window) {
        $scope.ExpenseTypeList = [];
        $scope.ExpenseType = {
            Id: 0,
            Name: "",
        }

        $scope.init = function (expenseTypeList) {

            $scope.ExpenseTypeList = expenseTypeList;
        }

        $scope.onClickAdd = function () {

            if ($scope.ExpenseType.Id == "") {
                data = $scope.ExpenseType;
                $http.post("/api/ExpenseType/Add", data).then(function (responsedata) {
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
                data = $scope.ExpenseType
                $http.post('/api/ExpenseType/Edit', data).then(function (responsedata) {
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
            $scope.ExpenseType = {
                Id: 0,
                Name: "",
            }

            $('#addModal').modal('show');
        }

        $scope.onClickCloseModal = function () {
            $('#addModal').modal('hide');
        }

        $scope.ShowEdit = function (id) {

            $http({
                url: "/api/ExpenseType/Edit",
                method: "GET",
                params: { id: id }
            }).then(function (responsedata) {
                console.log(responsedata.data);
                if (responsedata.data) {
                    var data = responsedata.data.Data;
                    $scope.ExpenseType.Id = data.Id;
                    $scope.ExpenseType.Name = data.Name;

                    $('#addModal').modal('show');
                }
                else {
                    alert(responsedata.data.Message);
                }
            });
          
        }

        $scope.Delete = function (id) {

            $http({
                url: "/api/ExpenseType/Delete",
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









