moduleGoldenDayCreateCampaign.controller('createCampaignController', ['$rootScope', '$scope', '$http', '$timeout', function ($rootScope, $scope, $http, $timeout) {
    $scope.getCampaignById = function(campaignId){
        $http({
            method: 'POST',
            url: 'WebService/GoldenDayCreateCampaignWebService.asmx/CampaignGetById',
            data: { campaignId: campaignId},
        }).then(function (response) {
            $rootScope.campaign = JSON.parse(response.data.d);
            setTimeout(function(){__doPostBack($('[data-id=btnSave]').attr('data-uniqueid'),'OnClick')},1);       
        }, function (response) {
            alert('Request failed. Please reload and try again. Message:' + response.data.Message);
        })
    }
    $scope.getGoldenDayById = function(goldenDayId){
        $http({
            method: 'POST',
            url: 'WebService/GoldenDayCreateCampaignWebService.asmx/GoldenDayGetById',
            data: { goldenDayId: goldenDayId},
        }).then(function (response) {
            $rootScope.goldenDay = JSON.parse(response.data.d).goldenDayDTO;
            $rootScope.policies = JSON.parse(response.data.d).policiesDTO; 
            $timeout(function () {
                autosize($('textarea'));
            })
            setInputMask();
        }, function (response) {
            alert('Request failed. Please reload and try again. Message:' + response.data.Message);
        })
    }
    $scope.pageLoad = function(){
        var campaignId = 0;
        try{
            campaignId = parseInt(getParameterValues('ci'));
        }catch(ex){}
        if(!campaignId) campaignId=0
        var goldenDayId = 0;
        try{
            goldenDayId = parseInt(getParameterValues('gdi'));
        }catch(ex){}
        if(!goldenDayId) goldenDayId=0
        if(campaignId != 0)
            $scope.getCampaignById(campaignId);
        if(goldenDayId != 0)
            $scope.getGoldenDayById(goldenDayId);    
    }
    $scope.pageLoad();
    $scope.buttonSaveDisabled = false;
    $scope.save = function () {
        $http({
            method: 'POST',
            url: 'WebService/GoldenDayCreateCampaignWebService.asmx/CreateCampaign',
            data: { month: $scope.month, year: $scope.year },
        }).then(function (response) {
            $rootScope.campaign = JSON.parse(response.data.d);
            addParameterToUrl('ci=' + $rootScope.campaign.Id);
        }, function (response) {
            alert('Request failed. Please reload and try again. Message:' + response.data.Message);
        }).finally(function(){
            $scope.buttonSaveDisabled = false;
        })
    }
    function setInputMask() {
        $timeout(function () {
            $("[data-control='inputmask']").inputmask({
                'alias': 'numeric',
                'groupSeparator': ',',
                'autoGroup': true,
                'digits': 2,
                'digitsOptional': true,
                'rightAlign': false
            })
            $('input[type="text"]').keydown(function () {
                $(this).trigger('input');
                $(this).trigger('change');
            });
        }, 0);
    }
}]);

moduleGoldenDayCreateCampaign.controller('createPolicyController', ['$rootScope', '$scope', '$http', '$timeout','$compile', function ($rootScope, $scope, $http, $timeout, $compile) {
    Sys.WebForms.PageRequestManager.getInstance().add_endRequest(function (sender, args) {
        var elem = angular.element(document.getElementById("upCruiseAvailable"));
        elem.replaceWith($compile(elem)($scope));
        $scope.$apply();
        setInputMask();
    });
    $rootScope.goldenDays = [];
    $scope.buttonSaveDisabled = false;
    $scope.add = function () {
        $rootScope.campaign.Policies.push({
            Adult:0,
            Child:0,
            Trip:{
                Id:'0'
            },
        });
        $timeout(function () {
            autosize($('textarea'));
        })
        setInputMask();
    };
    $scope.addGoldenDay = function(dateAsString){
        $rootScope.campaign.DateAsStrings.push(dateAsString);
    }
    $scope.removeGoldenDay = function(dateAsString){
        $rootScope.campaign.DateAsStrings.remove(dateAsString);
    }
    $scope.delete = function ($index) {
        $rootScope.campaign.Policies.splice($index, 1);
    };
    $scope.save = function (control) {    
        if ($("#aspnetForm").valid()) {
            var goldenDayId = 0;
            try{
                goldenDayId = parseInt(getParameterValues('gdi'));
            }catch(ex){}
            if(!goldenDayId) goldenDayId=0
            if(goldenDayId != 0){
                for(var i = 0 ; i < $rootScope.policies.length; i++){
                    $rootScope.policies[i].Adult = parseInt($rootScope.policies[i].Adult.toString().replace(/,/g,''));
                    $rootScope.policies[i].Child = parseInt($rootScope.policies[i].Child.toString().replace(/,/g,''));
                }
                $http({
                    method: 'POST',
                    url: 'WebService/GoldenDayCreateCampaignWebService.asmx/GoldenDaySaveOrUpdate',
                    data: { goldenDayDTO : $rootScope.goldenDay, policiesDTO : $rootScope.policies },
                }).then(function (response) {
                    window.location = window.location;
                }, function (response) {
                    alert('Request failed. Please reload and try again. Message:' + response.data.Message);
                }).finally(function(){
                    $scope.buttonSaveDisabled = false;
                });
            }
            if(goldenDayId == 0){
                for(var i = 0 ; i < $rootScope.campaign.Policies.length; i++){
                    $rootScope.campaign.Policies[i].Adult = parseInt($rootScope.campaign.Policies[i].Adult.toString().replace(/,/g,''));
                    $rootScope.campaign.Policies[i].Child = parseInt($rootScope.campaign.Policies[i].Child.toString().replace(/,/g,''));
                }
                $http({
                    method: 'POST',
                    url: 'WebService/GoldenDayCreateCampaignWebService.asmx/CampaignSaveOrUpdate',
                    data: { campaignDTO : $rootScope.campaign },
                }).then(function (response) {
                    window.location = 'GoldenDayListCampaign.aspx?NodeId=1&SectionId=15';
                }, function (response) {
                    alert('Request failed. Please reload and try again. Message:' + response.data.Message);
                }).finally(function(){
                    $scope.buttonSaveDisabled = false;
                });
            }
        }
    }
    $scope.isSelected = function(dateAsString){
        return $rootScope.campaign.DateAsStrings.some(d=>d.includes(dateAsString));
    }
    function setInputMask() {
        $timeout(function () {
            $("[data-control='inputmask']").inputmask({
                'alias': 'numeric',
                'groupSeparator': ',',
                'autoGroup': true,
                'digits': 2,
                'digitsOptional': true,
                'rightAlign': false
            })
            $('input[type="text"]').keydown(function () {
                $(this).trigger('input');
                $(this).trigger('change');
            });
        }, 0);
    }
    $scope.deletePolicyGoldenDay = function($index){
        $rootScope.policies.splice($index, 1);
    }
    $scope.addPolicyGoldenDay = function(){
        $rootScope.policies.push({
            Adult:0,
            Child:0,
            Trip:{
                Id:'0'
            },
        });
        $timeout(function () {
            autosize($('textarea'));
        })
        setInputMask();
    }
}])

