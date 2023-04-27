function printEstimate(pdf,fileName) {
    const linkSource = `data:application/pdf;base64,${pdf}`;
    const downloadLink = document.createElement("a");


    downloadLink.href = linkSource;
    downloadLink.download = fileName;
    downloadLink.click();
}

function getBase64(file) {
    console.log(file);
    var reader = new FileReader();
    reader.readAsDataURL(file);
    reader.onload = function () {
        console.log(reader.result);
    };
    reader.onerror = function (error) {
        console.log('Error: ', error);
    };
}

function downloadBlobLink(link,fileName) {
    var a = document.createElement("a"); //Create <a>
    a.href = link; //link to blob file
    a.download = fileName; //File name Here
    a.click();
}

function fileDownload(name, contentType, content) {
    // Convert the parameters to actual JS types
    const nameStr = BINDING.conv_string(name);
    const contentTypeStr = BINDING.conv_string(contentType);
    const contentArray = Blazor.platform.toUint8Array(content);

    // Create the URL
    const file = new File([contentArray], nameStr, { type: contentTypeStr });
    const exportUrl = URL.createObjectURL(file);

    // Create the <a> element and click on it
    const a = document.createElement("a");
    document.body.appendChild(a);
    a.href = exportUrl;
    a.download = nameStr;
    a.target = "_self";
    a.click();

    // We don't need to keep the url, let's release the memory
    // On Safari it seems you need to comment this line... (please let me know if you know why)
    URL.revokeObjectURL(exportUrl);
}