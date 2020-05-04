$(document).ready(function () {
    getAnswers();

    $("#vote-up-button").click(
        function (e) {
            e.preventDefault();
            sendVote("/Question/VoteUp");
		}
    );

    $("#vote-down-button").click(
        function (e) {
            e.preventDefault();
            sendVote("/Question/VoteDown");
        }
    );

    $("#send-answer-button").click(
        function (e) {
            e.preventDefault();
            sendAnswer();
        }
    );
});

function getAnswers() {
    $.ajax({
        url: "/Question/GetLastAnswers",
        type: "GET",
        dataType: "json",
        data: {
            questionId: $("#question-id").val(),
            amount: 10
        },
        success: function (response) {
            $("#answers").render(response);
            $("#answers").css("visibility", "visible");
        },
        error: function (response) {
            console.log("error", response);
        }
    });
}

function sendAnswer() {

    if ($("#answer-text").val() != "") {
        $.ajax({
            url: "/Question/AddAnswer",
            type: "POST",
            dataType: "json",
            data: {
                questionId: $("#question-id").val(),
                answerText: $("#answer-text").val()
            },
            success: function (response) {
                if (response != null) {
                    $("#answers").render(response);
                }
                $("#answer-text").val("");
            },
            error: function (response) {
                console.log("error", response);
            }
        });
    }
}

function sendVote(url) {
    $.ajax({
        url: url,
        type: "POST",
        dataType: "json",
        data: {
            questionId: $("#question-id").val()
        },
        success: function (response) {
            let result = $.parseJSON(response);
            $("#question-rating").text(response);
        },
        error: function (response) {
            console.log("error", response);
        }
    });
}