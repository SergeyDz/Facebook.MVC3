var App =
{
    initialize: function () {
        $("#Friends_btnCheckAllFriends").click(function () {
            var checked_status = "checked";
            $("input[name=chkFriends]").each(function () {
                this.checked = checked_status;
            });
        });

        $("#Friends_btnUnCheckAllFriends").click(function () {
            var checked_status = "";
            $("input[name=chkFriends]").each(function () {
                this.checked = checked_status;
            });
        });
    }

};

$(document).ready(function () {
    App.initialize();
});



