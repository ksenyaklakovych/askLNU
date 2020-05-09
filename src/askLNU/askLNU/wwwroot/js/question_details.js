$(document).ready(function () {
    getAnswers();

    $("#vote-up-button").click(
        function (e) {
            e.preventDefault();
            sendQuestionVote("/Question/VoteUp");
		}
    );

    $("#vote-down-button").click(
        function (e) {
            e.preventDefault();
            sendQuestionVote("/Question/VoteDown");
        }
    );

    $("#send-answer-button").click(
        function (e) {
            e.preventDefault();
            sendAnswer();
        }
    );
});

function bindVoteEvents() {
    $("#answers .row .answer-vote-up-button").on('click', function (e) {
        e.preventDefault();
        sendAnswerVote("/Answer/VoteUp", $(this).closest(".row"));
    });

    $("#answers .row .answer-vote-down-button").on("click", function (e) {
        e.preventDefault();
        sendAnswerVote("/Answer/VoteDown", $(this).closest(".row"));
    });
}

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
            bindVoteEvents();
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

function sendQuestionVote(url) {
    $.ajax({
        url: url,
        type: "POST",
        dataType: "json",
        data: {
            questionId: $("#question-id").val()
        },
        success: function (response) {
            $("#question-rating").text(response);
        },
        error: function (response) {
            console.log("error", response);
        }
    });
}

function sendAnswerVote(url, answerRow) {
    $.ajax({
        url: url,
        type: "POST",
        dataType: "json",
        data: {
            answerId: answerRow.find("[name='answer-id']").val()
        },
        success: function (response) {
            answerRow.find(".answer-rating").text(response);
        },
        error: function (response) {
            console.log("error", response);
        }
    });
}
