$(".tags-select").select2({
    minimumInputLength: 2,
    tags: true,
    ajax: {
        url: "/api/Tag/Find",
        dataType: 'json',
        type: "GET",
        quietMillis: 50,
        data: function (params) {
            return {
                searchString: params.term
            };
        },
        processResults: function (data) {
            var res = data.map(function (item) {
                return { text: item, id: item };
            });
            return {
                results: res
            };
        }
    }
});

$(".faculty-select").select2();