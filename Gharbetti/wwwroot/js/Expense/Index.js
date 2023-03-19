﻿var app = angular.module('expenseIndex', []);


app.controller('expenseController', ['$scope', '$filter', '$compile', '$http', '$rootScope', '$timeout', '$q', '$log', '$window',
    function ($scope, $filter, $compile, $http, $rootScope, $timeout, $q, $log, $window) {
        $scope.TransactionList = [];
        $scope.AdminTransactionList = [];
        $scope.PaymentModeList = [];
        $scope.ExpenseType = [];
        $scope.RoomModalTitle = "";


        $scope.ExpenseDetail = [];
        $scope.StartDateString = "";
        $scope.EndDateString = "";

        $scope.FilterData = {
            FilterType: 0,
            Month: "0",
            Year: "2023",
            StartDateString: "",
            EndDateString: ""
        }
        $scope.CurrentRole = "";

        $scope.IsVisible = {
            Tenant : false,
            Admin : false,
        };

        $scope.init = function (currentRole) {

            $scope.CurrentRole = currentRole;

            if ($filter('lowercase')($scope.CurrentRole) == "tenant") {
                $scope.IsVisible.Tenant = true;
            }
            else if ($filter('lowercase')($scope.CurrentRole) == "admin") {
                $scope.IsVisible.Admin = true;
            }


            $('#modalStartDate').datepicker({
                format: 'mm/dd/yyyy',
                autoclose: true,
                todayHighlight: true,
                todayBtn: true

            });
            $('#modalEndDate').datepicker({
                format: 'mm/dd/yyyy',
                autoclose: true,
                todayHighlight: true,
                todayBtn: true
            });


            $scope.GetExpenseType();
            $scope.onChangeFilterType();
        }

        $scope.onClickFilter = function () {
            $scope.TransactionList = [];
            $scope.AdminTransactionList = [];

            if ($filter('lowercase')($scope.CurrentRole) == "admin") {
                $http.post("/api/Expense/GetLandlordRange", $scope.FilterData).then(function (responseData) {
                    if (responseData.data.Status) {
                        $scope.AdminTransactionList = responseData.data.Data;
                    }
                    else {
                        alert(responseData.data.Message);
                    }
                });
            }
            else if ($filter('lowercase')($scope.CurrentRole) == "tenant") {

                $http.post("/api/Expense/GetTenantRange", $scope.FilterData).then(function (responseData) {
                    if (responseData.data.Status) {
                        $scope.TransactionList = responseData.data.Data;
                    }
                    else {
                        alert(responseData.data.Message);
                    }
                });
            }
        }


        $scope.ShowExpense = function (id) {
            $scope.ExpenseDetail = [];
            $http({
                url: "/api/Expense/ShowTranDetail",
                method: "GET",
                params: { id: id }
            }).then(function (responsedata) {
                debugger;
                console.log(responsedata.data);
                if (responsedata.data) {
                    var data = responsedata.data.Data;
                    $scope.ExpenseDetail = data;

                    angular.forEach($scope.ExpenseDetail, function (value, item) {
                        value.ExpenseId = $scope.ExpenseType.find(x => x.Id == value.ExpenseId);
                    });

                    $('#addModal').modal('show');
                }
                else {
                    alert(responsedata.data.Message);
                }
            });

        }

        $scope.GetExpenseType = function () {
            $http.get('/api/expensetype').then(function (responsedata) {
                console.log(responsedata.data);
                if (responsedata.data) {
                    $scope.ExpenseType = responsedata.data.Data;
                }
            });
        }

        $scope.GetPaymentMode = function () {
            $http.get('/api/PaymentMode').then(function (responsedata) {
                console.log(responsedata.data);
                if (responsedata.data) {
                    $scope.PaymentModeList = responsedata.data.Data;
                }
            });
        }

        $scope.onChangeFilterType = function () {
            if ($scope.FilterData.FilterType == "0") {
                $scope.monthWise = true;
                $scope.filterRange = false;
            }
            else {
                $scope.monthWise = false;
                $scope.filterRange = true;
            }
        }
    }]);









