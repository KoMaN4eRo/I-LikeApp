var globalId;
$('#input').on('paste', function () {
    setTimeout(function () {
        //console.log($('#input').val());
        console.log($('#input').val().replace("https://vk.com/id", ""));
        VK($('#input').val().replace("https://vk.com/id", ""));
    }, 10);
});
function VK(x) {
    if ($("#input").val() == "") {
        return false;
    }
    ($('#input').val().replace("https://vk.com/", "").split("id").length != 2) ? getUser($('#input').val().replace("https://vk.com/", "").split("id")[0]) : searchVk($('#input').val().replace("https://vk.com/", "").split("id")[1]);
    document.getElementsByTagName("head")[0].appendChild(script);
}

function yellow(result) {
    var users = result.response;
    console.log("This is yellow function");
    globalId = result.response[0].id;
    console.log(globalId);
    $("#second").attr("src", result.response[0].photo_50);
}

function searchVk(x) {
    var script = document.createElement('SCRIPT');
    console.log("This is searchVk function");
    script.src = "https://api.vk.com/method/users.get?user_id=" + x + "&fields=photo_50&name_case=nom&v=5.62&callback=yellow";
    document.getElementsByTagName("head")[0].appendChild(script);
}

function getUser(x) {
    var t = "@Model.access_token";
    var script = document.createElement('SCRIPT');
    console.log("This is getUser function");
    script.src = "https://api.vk.com/method/users.search?q=" + x + "&fields=photo_50&count=1&access_token=" + t + "&v=5.62&callback=searchCallBack";
    document.getElementsByTagName("head")[0].appendChild(script);
}

function searchCallBack(result) {
    globalId = result.response.items[0].id;
    console.log(globalId);
    $("#second").attr("src", result.response.items[0].photo_50);
}

function OnIndexCall(elem) {
    $(elem).attr('href', $(elem).attr('href') + '?id=' + globalId);
}

function callbackFunc(result) {        
        var p = document.getElementById('first');
    var users = result.response.items;
    var slides = $("#Slides > div");
    var j = 0;
    var sliderFill = function () {
        for ( var i = 0; i < slides.length; i++) {
            if (j != result.response.items.length) {
                slides.children().children('img').eq(i).attr("src", users[j].photo_50);
                slides.children().children('span')[i].innerHTML = users[j].first_name;
                $(slides[i]).css("display", "block");
                j++;
            } 
        }
        console.log(j);
    }
    $(slides).children('a').on('click', function (e) {
        e.preventDefault();
    });
    sliderFill();
    $('.next').on("click", function () {
        if (j != users.length) {
            slides.css("display", "none");               
            sliderFill();
        }
    });

    $('.prev').on("click", function () {
        if (j > slides.length  ) {
            j = j  - slides.length;
            for (var i = 0; i < slides.length; i++) {
                if ($(slides[i]).css("display") == "block") { j--; }                   
            }
            sliderFill();
        }
        else {
            j = 0;
        }
    });

    $(slides.children().children('img')).on("click", function () {
        $(".more-info--obgImg > img").attr("src", $(this).attr("src"));
        $('.info > p').text("Lorem ipsum dolor sit amet, consectetur adipisicing elit. Aut dolorem inventore labore voluptate magni eum cumque quos cupiditate velit libero, aspernatur itaque ipsa voluptas soluta tenetur molestiae voluptatem quidem nihil.");
    });
    $(slides.children().children('span')).on("click", function () {
        $(".more-info--obgImg > img").attr("src", $(this).prev().attr("src"));
        $('.info > p').text("Lorem ipsum dolor sit amet, consectetur adipisicing elit. Aut dolorem inventore labore voluptate magni eum cumque quos cupiditate velit libero, aspernatur itaque ipsa voluptas soluta tenetur molestiae voluptatem quidem nihil.");
    });
}
