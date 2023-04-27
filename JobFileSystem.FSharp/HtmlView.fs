namespace JobFileSystem.FSharp

open JobFileSystem.Shared.Estimates
open System.Net.Http
open System
open System.IO

module HtmlView =
    open Giraffe.ViewEngine

    let private estimateReport (estimate: EstimateDto) =
        
        let lineItems = 
            [
                for item in estimate.LineItems do
                    yield $"""
                    <tr>
                        <th style="width: 100px;">{item.Qty.ToString("N2")}</th>
                        <td style="width: 100px;">{item.UnitOfMeasure}</td>
                        <td>{item.Description}</td>
                        <td style="width: 100px;">{item.Delivery}</td>
                        <td style="width: 100px;">${item.UnitPrice.ToString("N2")}</td>
                        <td style="width: 100px;">${item.LineTotal.ToString("N2")}</td>
                    </tr>
                    """
            ]
            |> String.concat ""
        

        let html = 
            $"""
            <!DOCTYPE html>
            
            <html lang="en" >
            <head>
                <meta content="text/html; charset=utf-8" http-equiv="Content-Type" />
                <meta content="width=device-width, initial-scale=1.0" name="viewport" />
                <link href="https://cdn.jsdelivr.net/npm/bootstrap@5.0.2/dist/css/bootstrap.min.css" rel="stylesheet" integrity="sha384-EVSTQN3/azprG1Anm3QDgpJLIm9Nao0Yz1ztcQTwFspd3yD65VohhpuuCOmLASjC" crossorigin="anonymous">
                <script src="https://cdn.jsdelivr.net/npm/bootstrap@5.0.2/dist/js/bootstrap.bundle.min.js" integrity="sha384-MrcW6ZMFYlzcLA8Nl+NtUVF0sA7MsXsP1UyJoMp4YLEuNSfAP+JcXn/tWtIaxVXM" crossorigin="anonymous"></script>
                
            </head>
            <body class="container-fluid" >
                <table class="table1" border="0" cellpadding="0" cellspacing="0" class="nl-container" role="presentation" >
                    <tbody>
                        <tr>
                            <td>
                                <table align="center" border="0" cellpadding="0" cellspacing="0" class="row row-1" role="presentation" style="mso-table-lspace: 0pt; mso-table-rspace: 0pt;" width="100%%">
                                    <tbody>
                                        <tr>
                                            <td>
                                                <table align="center" border="0" cellpadding="0" cellspacing="0" class="row-content stack" role="presentation" style="mso-table-lspace: 0pt; mso-table-rspace: 0pt; color: #000000; width: 800px;" width="800">
                                                    <tbody>
                                                        <tr>
                                                            <td class="column column-1" style="mso-table-lspace: 0pt; mso-table-rspace: 0pt; font-weight: 400; text-align: left; vertical-align: top; border-top: 0px; border-right: 0px; border-bottom: 0px; border-left: 0px;" width="50%%">
                                                                <table border="0" cellpadding="0" cellspacing="0" class="image_block" role="presentation" style="mso-table-lspace: 0pt; mso-table-rspace: 0pt;" width="100%%">
                                                                    <tr>
                                                                        <td style="width:100%%;padding-right:0px;padding-left:0px;padding-top:5px;padding-bottom:5px;">
                                                                            <div align="left" style="line-height:10px"><img class="big" style="width:200px;margin-top: 0.5rem;" src="https://jobfilesystem.blob.core.windows.net/logos/ARMM_EMAIL_LOGO.png?sp=r&st=2022-07-02T13:29:05Z&se=2030-01-01T22:29:05Z&spr=https&sv=2021-06-08&sr=b&sig=XMJ%%2FclF4vYWFxpu1nBf7fEktkBajQHMEmgpZyHPg1fE%%3D" style="display: block; height: auto; border: 0; width: 300px; max-width: 100%%;height: 75px;" width="400" /></div>
                                                                        </td>
                                                                    </tr>
                                                                </table>
                                                            </td>
                                                            <td class="column column-2" style="mso-table-lspace: 0pt; mso-table-rspace: 0pt; font-weight: 400; text-align: left; vertical-align: top; border-top: 0px; border-right: 0px; border-bottom: 0px; border-left: 0px;" width="50%%">
                                                                <table border="0" cellpadding="0" cellspacing="0" class="heading_block" role="presentation" style="mso-table-lspace: 0pt; mso-table-rspace: 0pt;" width="100%%">
                                                                    <tr>
                                                                        <td style="width:100%%;text-align:center;padding-top:35px;padding-bottom:5px;">
                                                                            <h1 style="margin: 0; color: #323f57; font-size: 34px; font-family: Arial, Helvetica Neue, Helvetica, sans-serif; line-height: 100%%; text-align: right; direction: ltr; font-weight: 700; letter-spacing: normal; margin-top: 0; margin-bottom: 0; margin-right: 3.5rem;"><span class="mt-auto mb-auto">ESTIMATE</span></h1>
                                                                        </td>
                                                                    </tr>
                                                                </table>
                                                            </td>
                                                        </tr>
                                                    </tbody>
                                                </table>
                                            </td>
                                        </tr>
                                    </tbody>
                                </table>
                                <table align="center" border="0" cellpadding="0" cellspacing="0" class="row row-2" role="presentation" style="mso-table-lspace: 0pt; mso-table-rspace: 0pt;" width="100%%">
                                    <tbody>
                                        <tr>
                                            <td>
                                                <table align="center" border="0" cellpadding="0" cellspacing="0" class="row-content stack" role="presentation" style="mso-table-lspace: 0pt; mso-table-rspace: 0pt; color: #000000; width: 800px;" width="800">
                                                    <tbody>
                                                        <tr>
                                                            <td class="column column-1" style="mso-table-lspace: 0pt; mso-table-rspace: 0pt; font-weight: 400; text-align: left; vertical-align: top; border-top: 0px; border-right: 0px; border-bottom: 0px; border-left: 0px;" width="58.333333333333336%%">
                                                                <table border="0" cellpadding="0" cellspacing="0" class="empty_block" role="presentation" style="mso-table-lspace: 0pt; mso-table-rspace: 0pt;" width="100%%">
                                                                    <tr>
                                                                        <td style="padding-right:0px;padding-bottom:5px;padding-left:0px;padding-top:5px;">
                                                                            <div></div>
                                                                        </td>
                                                                    </tr>
                                                                </table>
                                                            </td>
                                                            <td class="column column-2" style="mso-table-lspace: 0pt; mso-table-rspace: 0pt; font-weight: 400; text-align: left; vertical-align: top; border-top: 0px; border-right: 0px; border-bottom: 0px; border-left: 0px;" width="41.666666666666664%%">
                                                                <table border="0" cellpadding="0" cellspacing="0" class="paragraph_block" role="presentation" style="mso-table-lspace: 0pt; mso-table-rspace: 0pt; word-break: break-word;" width="100%%">
                                                                    <tr>
                                                                        <td style="padding-top:5px;padding-bottom:5px;">
                                                                            <div style="color:#000000;font-size:15px;font-family:Arial, Helvetica Neue, Helvetica, sans-serif;font-weight:400;line-height:120%%;text-align:justify;direction:ltr;letter-spacing:0px;mso-line-height-alt:18px;">
                                                                                <!-- ESTIMATE NUMBER -->
                                                                                <p style="margin: 0; margin-bottom: 0px;"><a name="ESTIMATE!E3" style="color: #0068a5;"></a><strong>ESTIMATE NO</strong>:<strong> </strong>{estimate.Number}</p>
                                                                                <!-- ESTIMATE DATE -->
                                                                                <p style="margin: 0;"><strong>DATE:</strong> {estimate.LoggedOn}</p>
                                                                                <!-- DELIVERY DATE -->
                                                                                <p style="margin: 0;"><strong>EST DELIVERY:</strong> {estimate.DeliveryDate.Value.ToString("yyyy-MM-dd")}</p>
                                                                            </div>
                                                                        </td>
                                                                    </tr>
                                                                </table>
                                                            </td>
                                                        </tr>
                                                    </tbody>
                                                </table>
                                            </td>
                                        </tr>
                                    </tbody>
                                </table>
                                <table align="center" border="0" cellpadding="0" cellspacing="0" class="row row-3" role="presentation" style="mso-table-lspace: 0pt; mso-table-rspace: 0pt;" width="100%%">
                                    <tbody>
                                        <tr>
                                            <td>
                                                <table align="center" border="0" cellpadding="0" cellspacing="0" class="row-content stack" role="presentation" style="mso-table-lspace: 0pt; mso-table-rspace: 0pt; color: #000000; width: 800px;" width="800">
                                                    <tbody>
                                                        <tr>
                                                            <td class="column column-1" style="mso-table-lspace: 0pt; mso-table-rspace: 0pt; font-weight: 400; text-align: left; vertical-align: top; padding-top: 5px; padding-bottom: 5px; border-top: 0px; border-right: 0px; border-bottom: 0px; border-left: 0px;" width="100%%">
                                                                <table border="0" cellpadding="0" cellspacing="0" class="paragraph_block" role="presentation" style="mso-table-lspace: 0pt; mso-table-rspace: 0pt; word-break: break-word;" width="100%%">
                                                                    <tr>
                                                                        <td>
                                                                            <div style="color:#000000;font-size:20px;font-family:Arial, Helvetica Neue, Helvetica, sans-serif;font-weight:400;line-height:120%%;text-align:left;direction:ltr;letter-spacing:0px;mso-line-height-alt:24px;">
                                                                                <!-- ESTIMATE CREATOR NAME -->
                                                                                <p style="margin: 0;"><strong>Marwan Asiff</strong></p>
                                                                            </div>
                                                                        </td>
                                                                    </tr>
                                                                </table>
                                                                <table border="0" cellpadding="0" cellspacing="0" class="paragraph_block" role="presentation" style="mso-table-lspace: 0pt; mso-table-rspace: 0pt; word-break: break-word;" width="100%%">
                                                                    <tr>
                                                                        <td>
                                                                            <div style="color:#000000;font-size:14px;font-family:Arial, Helvetica Neue, Helvetica, sans-serif;font-weight:400;line-height:120%%;text-align:left;direction:ltr;letter-spacing:0px;mso-line-height-alt:16.8px;">
                                                                                <p style="margin: 0; margin-bottom: 0px;">780.945.3845</p>
                                                                                <p style="margin: 0;"><a href="mailto:info@armm-services.com" style="color: #0068a5;">info@armm-services.com</a></p>
                                                                            </div>
                                                                        </td>
                                                                    </tr>
                                                                </table>
                                                            </td>
                                                        </tr>
                                                    </tbody>
                                                </table>
                                            </td>
                                        </tr>
                                    </tbody>
                                </table>
                                <table align="center" border="0" cellpadding="0" cellspacing="0" class="row row-4" role="presentation" style="mso-table-lspace: 0pt; mso-table-rspace: 0pt;" width="100%%">
                                    <tbody>
                                        <tr>
                                            <td>
                                                <table align="center" border="0" cellpadding="0" cellspacing="0" class="row-content stack" role="presentation" style="mso-table-lspace: 0pt; mso-table-rspace: 0pt; color: #000000; width: 800px;" width="800">
                                                    <tbody>
                                                        <tr>
                                                            <td class="column column-1" style="mso-table-lspace: 0pt; mso-table-rspace: 0pt; font-weight: 400; text-align: left; vertical-align: top; padding-top: 5px; padding-bottom: 5px; border-top: 0px; border-right: 0px; border-bottom: 0px; border-left: 0px;" width="100%%">
                                                                <table border="0" cellpadding="0" cellspacing="0" class="paragraph_block" role="presentation" style="mso-table-lspace: 0pt; mso-table-rspace: 0pt; word-break: break-word;" width="100%%">
                                                                    <tr>
                                                                        <td style="padding-top:10px;padding-right:10px;padding-left:10px;">
                                                                            <div style="color:#000000;font-size:14px;font-family:Arial, Helvetica Neue, Helvetica, sans-serif;font-weight:400;line-height:120%%;text-align:left;direction:ltr;letter-spacing:0px;mso-line-height-alt:16.8px;">
                                                                                <p style="margin: 0;"><strong>TO</strong></p>
                                                                            </div>
                                                                        </td>
                                                                    </tr>
                                                                </table>
                                                            </td>
                                                        </tr>
                                                    </tbody>
                                                </table>
                                            </td>
                                        </tr>
                                    </tbody>
                                </table>
                                <table align="center" border="0" cellpadding="0" cellspacing="0" class="row row-5" role="presentation" style="mso-table-lspace: 0pt; mso-table-rspace: 0pt;" width="100%%">
                                    <tbody>
                                        <tr>
                                            <td>
                                                <table align="center" border="0" cellpadding="0" cellspacing="0" class="row-content stack" role="presentation" style="mso-table-lspace: 0pt; mso-table-rspace: 0pt; color: #000000; width: 800px;" width="800">
                                                    <tbody>
                                                        <tr>
                                                            <td class="column column-1" style="mso-table-lspace: 0pt; mso-table-rspace: 0pt; font-weight: 400; text-align: left; vertical-align: top; padding-left: 30px; border-top: 0px; border-right: 0px; border-bottom: 0px; border-left: 0px;" width="58.333333333333336%%">
                                                                <table border="0" cellpadding="0" cellspacing="0" class="paragraph_block" role="presentation" style="mso-table-lspace: 0pt; mso-table-rspace: 0pt; word-break: break-word;" width="100%%">
                                                                    <tr>
                                                                        <td style="padding-top:15px;padding-right:15px;padding-bottom:20px;padding-left:15px;">
                                                                            <div style="color:#000000;font-size:12px;font-family:Arial, Helvetica Neue, Helvetica, sans-serif;font-weight:400;line-height:120%%;text-align:left;direction:ltr;letter-spacing:0px;mso-line-height-alt:16.8px;">
                                                                                <!-- CONTACT INFORMATION-->
                                                                                <!-- NAME-->
                                                                                <p style="margin: 0; margin-bottom: 0px;">{estimate.Client.Name}</p>
                                                                                <!-- COMPANY-->
                                                                                <p style="margin: 0; margin-bottom: 0px;">{estimate.Client.Company}</p>
                                                                                <!-- EMAIL-->
                                                                                <p style="margin: 0;">{estimate.Client.Email}</p>
                                                                                <!-- PHONE-->
                                                                                <p style="margin: 0;">{estimate.Client.Phone}</p>
                                                                            </div>
                                                                        </td>
                                                                    </tr>
                                                                </table>
                                                            </td>
                                                            <td class="column column-2" style="mso-table-lspace: 0pt; mso-table-rspace: 0pt; font-weight: 400; text-align: left; vertical-align: top; border-top: 0px; border-right: 0px; border-bottom: 0px; border-left: 0px;" width="41.666666666666664%%">
                                                                <table border="0" cellpadding="0" cellspacing="0" class="empty_block" role="presentation" style="mso-table-lspace: 0pt; mso-table-rspace: 0pt;" width="100%%">
                                                                    <tr>
                                                                        <td style="padding-right:0px;padding-bottom:5px;padding-left:0px;padding-top:5px;">
                                                                            <div></div>
                                                                        </td>
                                                                    </tr>
                                                                </table>
                                                            </td>
                                                        </tr>
                                                    </tbody>
                                                </table>
                                            </td>
                                        </tr>
                                    </tbody>
                                </table>
                                <table align="center" border="0" cellpadding="0" cellspacing="0" class="row row-6" role="presentation" style="mso-table-lspace: 0pt; mso-table-rspace: 0pt;" width="100%%">
                                    <tbody>
                                        <tr>
                                            <td>
                                                <table align="center" border="0" cellpadding="0" cellspacing="0" class="row-content stack" role="presentation" style="mso-table-lspace: 0pt; mso-table-rspace: 0pt; color: #000000; width: 800px;" width="800">
                                                    <tbody>
                                                        <tr>
                                                            <td class="column column-1" style="mso-table-lspace: 0pt; mso-table-rspace: 0pt; font-weight: 400; text-align: left; vertical-align: top; padding-top: 5px; padding-bottom: 5px; border-top: 0px; border-right: 0px; border-bottom: 0px; border-left: 0px;" width="100%%">
                                                                <table border="0" cellpadding="0" cellspacing="0" class="empty_block" role="presentation" style="mso-table-lspace: 0pt; mso-table-rspace: 0pt;" width="100%%">
                                                                    <tr>
                                                                        <td>
                                                                            <div></div>
                                                                        </td>
                                                                    </tr>
                                                                </table>
                                                            </td>
                                                        </tr>
                                                    </tbody>
                                                </table>
                                            </td>
                                        </tr>
                                    </tbody>
                                </table>
                                <table align="center" border="0" cellpadding="0" cellspacing="0" class="row row-7" role="presentation" style="mso-table-lspace: 0pt; mso-table-rspace: 0pt;" width="100%%">
                                    <tbody>
                                        <tr>
                                            <td>
                                                <table align="center" border="0" cellpadding="0" cellspacing="0" class="row-content stack" role="presentation" style="mso-table-lspace: 0pt; mso-table-rspace: 0pt; color: #000000; width: 800px;" width="800">
                                                    <tbody>
                                                        <tr>
                                                            <td class="column column-1" style="mso-table-lspace: 0pt; mso-table-rspace: 0pt; font-weight: 400; text-align: left; vertical-align: top; padding-top: 5px; padding-bottom: 5px; border-top: 0px; border-right: 0px; border-bottom: 0px; border-left: 0px;" width="100%%">
                                                                <table border="0" cellpadding="10" cellspacing="0" class="paragraph_block" role="presentation" style="mso-table-lspace: 0pt; mso-table-rspace: 0pt; word-break: break-word;" width="100%%">
                                                                    <tr>
                                                                        <td>
                                                                            <div style="color:#000000;font-size:12px;font-family:Arial, Helvetica Neue, Helvetica, sans-serif;font-weight:400;line-height:120%%;text-align:left;direction:ltr;letter-spacing:0px;mso-line-height-alt:16.8px;">
                                                                                <!-- Long Description -->
                                                                                <p style="margin: 0;">{estimate.LongDescription}</p>
                                                                            </div>
                                                                        </td>
                                                                    </tr>
                                                                </table>
                                                            </td>
                                                        </tr>
                                                    </tbody>
                                                </table>
                                            </td>
                                        </tr>
                                    </tbody>
                                </table>
                                <table align="center" border="0" cellpadding="0" cellspacing="0" class="row row-8" role="presentation" style="mso-table-lspace: 0pt; mso-table-rspace: 0pt;" width="100%%">
                                    <tbody>
                                        <tr>
                                            <td>
                                                <table align="center" border="0" cellpadding="0" cellspacing="0" class="row-content stack" role="presentation" style="mso-table-lspace: 0pt; mso-table-rspace: 0pt; color: #000000; width: 800px;" width="800">
                                                    <tbody>
                                                        <tr>
                                                            <td class="column column-1" style="mso-table-lspace: 0pt; mso-table-rspace: 0pt; font-weight: 400; text-align: left; vertical-align: top; padding-top: 5px; padding-bottom: 5px; border-top: 0px; border-right: 0px; border-bottom: 0px; border-left: 0px;" width="100%%">
                                                                <table border="0" cellpadding="0" cellspacing="0" class="html_block" role="presentation" style="mso-table-lspace: 0pt; mso-table-rspace: 0pt;" width="100%%">
                                                                    <tr>
                                                                        <td>
                                                                            <div align="left" style="font-family:Arial, Helvetica Neue, Helvetica, sans-serif;text-align:left;font-size: 10px;">
                                                                                <table class="table table-striped">
                                                                                    <thead>
                                                                                        <tr>
                                                                                            <th>QTY</th>
                                                                                            <th>UNIT</th>
                                                                                            <th>DESCRIPTION</th>
                                                                                            <th>DELIVERY</th>
                                                                                            <th>UNIT PRICE</th>
                                                                                            <th>LINE TOTAL</th>
                                                                                        </tr>
                                                                                    </thead>
            
                                                                                    <tbody>
                                                                                        <!-- LINE ITEMS-->
                                                                                        {
                                                                                           lineItems 
                                                                                        }
                                                                                        
                                                                                    </tbody>
                                                                                </table>
                                                                            </div>
                                                                        </td>
                                                                    </tr>
                                                                </table>
                                                            </td>
                                                        </tr>
                                                    </tbody>
                                                </table>
                                            </td>
                                        </tr>
                                    </tbody>
                                </table>
                                <table align="center" border="0" cellpadding="0" cellspacing="0" class="row row-9" role="presentation" style="mso-table-lspace: 0pt; mso-table-rspace: 0pt;" width="100%%">
                                    <tbody>
                                        <tr>
                                            <td>
                                                <table align="center" border="0" cellpadding="0" cellspacing="0" class="row-content stack" role="presentation" style="mso-table-lspace: 0pt; mso-table-rspace: 0pt; color: #000000; width: 800px;" width="800">
                                                    <tbody>
                                                        <tr>
                                                            <td class="column column-1" style="mso-table-lspace: 0pt; mso-table-rspace: 0pt; font-weight: 400; text-align: left; vertical-align: top; border-top: 0px; border-right: 0px; border-bottom: 0px; border-left: 0px;" width="33.333333333333336%%">
                                                                <table border="0" cellpadding="0" cellspacing="0" class="empty_block" role="presentation" style="mso-table-lspace: 0pt; mso-table-rspace: 0pt;" width="100%%">
                                                                    <tr>
                                                                        <td style="padding-right:0px;padding-bottom:5px;padding-left:0px;padding-top:5px;">
                                                                            <div></div>
                                                                        </td>
                                                                    </tr>
                                                                </table>
                                                            </td>
                                                            <td class="column column-2" style="mso-table-lspace: 0pt; mso-table-rspace: 0pt; font-weight: 400; text-align: left; vertical-align: top; border-top: 0px; border-right: 0px; border-bottom: 0px; border-left: 0px;" width="33.333333333333336%%">
                                                                <table border="0" cellpadding="0" cellspacing="0" class="empty_block" role="presentation" style="mso-table-lspace: 0pt; mso-table-rspace: 0pt;" width="100%%">
                                                                    <tr>
                                                                        <td style="padding-right:0px;padding-bottom:5px;padding-left:0px;padding-top:5px;">
                                                                            <div></div>
                                                                        </td>
                                                                    </tr>
                                                                </table>
                                                            </td>
                                                            <td class="column column-3" style="mso-table-lspace: 0pt; mso-table-rspace: 0pt; font-weight: 400; text-align: left; vertical-align: top; border-top: 0px; border-right: 0px; border-bottom: 0px; border-left: 0px;" width="33.333333333333336%%">
                                                                <table border="0" cellpadding="0" cellspacing="0" class="paragraph_block" role="presentation" style="mso-table-lspace: 0pt; mso-table-rspace: 0pt; word-break: break-word;" width="100%%">
                                                                    <tr>
                                                                        <td style="padding-top:15px;padding-right:10px;padding-bottom:15px;padding-left:10px;">
                                                                            <div style="color:#000000;font-size:10px;font-family:Arial, Helvetica Neue, Helvetica, sans-serif;font-weight:400;line-height:120%%;text-align:right;direction:ltr;letter-spacing:0px;mso-line-height-alt:16.8px;">
                                                                                <p style="margin: 0; margin-bottom: 7px;"><u><strong>Sub Total:</strong>${estimate.Subtotal.ToString("N2")}</u></p>
                                                                                <p style="margin: 0; margin-bottom: 7px;"><strong>GST:</strong>${estimate.Gst.ToString("N2")}</p>
                                                                                <p style="margin: 0;"><u><strong style="border-bottom: double black;">Total:</strong> <strong style="border-bottom: double black;">${estimate.Total.ToString("N2")}</strong></u></p>
                                                                            </div>
                                                                        </td>
                                                                    </tr>
                                                                </table>
                                                            </td>
                                                        </tr>
                                                    </tbody>
                                                </table>
                                            </td>
                                        </tr>
                                    </tbody>
                                </table>
                                <table align="center" border="0" cellpadding="0" cellspacing="0" class="row row-10" role="presentation" style="mso-table-lspace: 0pt; mso-table-rspace: 0pt;" width="100%%">
                                    <tbody>
                                        <tr>
                                            <td>
                                                <table align="center" border="0" cellpadding="0" cellspacing="0" class="row-content stack" role="presentation" style="mso-table-lspace: 0pt; mso-table-rspace: 0pt; color: #000000; width: 800px;" width="800">
                                                    <tbody>
                                                        <tr>
                                                            <td class="column column-1" style="mso-table-lspace: 0pt; mso-table-rspace: 0pt; font-weight: 400; text-align: left; vertical-align: top; padding-top: 5px; padding-bottom: 5px; border-top: 0px; border-right: 0px; border-bottom: 0px; border-left: 0px;" width="100%%">
                                                                <div class="spacer_block" style="height:60px;line-height:60px;font-size:1px;"> </div>
                                                            </td>
                                                        </tr>
                                                    </tbody>
                                                </table>
                                            </td>
                                        </tr>
                                    </tbody>
                                </table>
                                <table align="center" border="0" cellpadding="0" cellspacing="0" class="row row-11" role="presentation" style="mso-table-lspace: 0pt; mso-table-rspace: 0pt;" width="100%%">
                                    <tbody>
                                        <tr>
                                            <td>
                                                <table align="center" border="0" cellpadding="0" cellspacing="0" class="row-content stack" role="presentation" style="mso-table-lspace: 0pt; mso-table-rspace: 0pt; color: #000000; width: 800px;" width="800">
                                                    <tbody>
                                                        <tr>
                                                            <td class="column column-1" style="mso-table-lspace: 0pt; mso-table-rspace: 0pt; font-weight: 400; text-align: left; vertical-align: top; padding-top: 5px; padding-bottom: 5px; border-top: 0px; border-right: 0px; border-bottom: 0px; border-left: 0px;" width="100%%">
                                                                <table border="0" cellpadding="0" cellspacing="0" class="paragraph_block" role="presentation" style="mso-table-lspace: 0pt; mso-table-rspace: 0pt; word-break: break-word;" width="100%%">
                                                                    <tr>
                                                                        <td style="padding-top:10px;padding-right:10px;padding-left:10px;">
                                                                            <div style="color:#000000;font-size:10px;font-family:Arial, Helvetica Neue, Helvetica, sans-serif;font-weight:400;line-height:120%%;text-align:left;direction:ltr;letter-spacing:0px;mso-line-height-alt:14.399999999999999px;">
                                                                                <p style="margin: 0;"><strong>This is a quotation on the goods named, subject to the conditions noted below:</strong></p>
                                                                            </div>
                                                                        </td>
                                                                    </tr>
                                                                </table>
                                                            </td>
                                                        </tr>
                                                    </tbody>
                                                </table>
                                            </td>
                                        </tr>
                                    </tbody>
                                </table>
                                <table align="center" border="0" cellpadding="0" cellspacing="0" class="row row-12" role="presentation" style="mso-table-lspace: 0pt; mso-table-rspace: 0pt;" width="100%%">
                                    <tbody>
                                        <tr>
                                            <td>
                                                <table align="center" border="0" cellpadding="0" cellspacing="0" class="row-content stack" role="presentation" style="mso-table-lspace: 0pt; mso-table-rspace: 0pt; color: #000000; width: 800px;" width="800">
                                                    <tbody>
                                                        <tr>
                                                            <td class="column column-1" style="mso-table-lspace: 0pt; mso-table-rspace: 0pt; font-weight: 400; text-align: left; vertical-align: top; padding-top: 5px; padding-bottom: 5px; border-top: 0px; border-right: 0px; border-bottom: 0px; border-left: 0px;" width="100%%">
                                                                <table border="0" cellpadding="0" cellspacing="0" class="paragraph_block" role="presentation" style="mso-table-lspace: 0pt; mso-table-rspace: 0pt; word-break: break-word;" width="100%%">
                                                                    <tr>
                                                                        <td style="padding-right:10px;padding-bottom:10px;padding-left:35px;">
                                                                            <div style="color:#000000;font-size:8px;font-family:Arial, Helvetica Neue, Helvetica, sans-serif;font-weight:400;line-height:120%%;text-align:left;direction:ltr;letter-spacing:0px;mso-line-height-alt:13.2px;">
                                                                                <p style="margin: 0;">
                                                                                    1. Pricing is in Canadian funds. 
                                                                                        <br />
                                                                                    2. Prices are firm for 30 days.
                                                                                        <br />
                                                                                    3. Terms: Net 30/2%% Charges per month thereafter. 
                                                                                        <br />
                                                                                    4. Payment Terms: Due upon receipt.
                                                                                        <br />
                                                                                    5. 3.5%% surcharge applies to all payments made via credit card.
                                                                                
                                                                                </p>
                                                                            </div>
                                                                        </td>
                                                                    </tr>
                                                                </table>
                                                            </td>
                                                        </tr>
                                                    </tbody>
                                                </table>
                                            </td>
                                        </tr>
                                    </tbody>
                                </table>
                                <table align="center" border="0" cellpadding="0" cellspacing="0" class="row row-13" role="presentation" style="padding-top:5rem;" width="100%%">
                                    <tbody>
                                        <tr>
                                            <td>
                                                <table align="center" border="0" cellpadding="0" cellspacing="0" class="row-content stack" role="presentation" style="mso-table-lspace: 0pt; mso-table-rspace: 0pt; color: #000000; width: 800px;" width="800">
                                                    <tbody>
                                                        <tr>
                                                            <td class="column column-1" style="mso-table-lspace: 0pt; mso-table-rspace: 0pt; font-weight: 400; text-align: left; vertical-align: top; padding-top: 5px; padding-bottom: 5px; border-top: 0px; border-right: 0px; border-bottom: 0px; border-left: 0px;" width="100%%">
                                                                <table border="0" cellpadding="0" cellspacing="0" class="icons_block" role="presentation" style="mso-table-lspace: 0pt; mso-table-rspace: 0pt;" width="100%%">
                                                                    <tr>
                                                                        <td style="vertical-align: middle; color: #9d9d9d; font-family: inherit; font-size: 15px; padding-bottom: 5px; padding-top: 5px; text-align: center;">
                                                                            <table cellpadding="0" cellspacing="0" role="presentation" style="mso-table-lspace: 0pt; mso-table-rspace: 0pt;" width="100%%">
                                                                                <tr>
                                                                                    <td style="vertical-align: middle; text-align: left;">
                                                                                        
                                                                                        <table cellpadding="0" cellspacing="0" class="icons-inner" role="presentation" style="mso-table-lspace: 0pt; mso-table-rspace: 0pt; display: inline-block; margin-right: -4px; padding-left: 0px; padding-right: 0px;">
                                                                                            

                                                                                            <tr>
                                                                                                <td style="width:100%%;padding-right:0px;padding-left:0px;padding-top:10rempx;padding-bottom:5px;">
                                                                                                    <div align="left" style="line-height:10px"><img class="big" style="width:100px;margin-top: 0.5rem;" src="https://jobfilesystem.blob.core.windows.net/logos/estimate_ccablogo.png?sp=r&st=2022-07-02T13:32:51Z&se=2030-07-02T21:32:51Z&spr=https&sv=2021-06-08&sr=b&sig=gWCAxc0Xvl1Akv1j9o87ebDivoGJsvq5dqMNFp0YeKo%%3D" style="display: block; height: auto; border: 0; width: 300px; max-width: 100%%;height: 75px;" width="400" /></div>
                                                                                                </td>
                                                                                            </tr>
                                                              


                                                                                        </table>
                                                                                    </td>
                                                                                </tr>
                                                                            </table>
                                                                        </td>
                                                                    </tr>
                                                                </table>
                                                            </td>
                                                        </tr>
                                                    </tbody>
                                                </table>
                                            </td>
                                        </tr>
                                    </tbody>
                                </table>
                            </td>
                        </tr>
                    </tbody>
                </table><!-- End -->
            </body>
            </html>
            """
        html

    let private print(client: HttpClient)(html:string) =
        async {
            let body = new StringContent(html)
            let uri =
                new Uri """https://htlmtopdf2021.azurewebsites.net/api/PrintPortrait?code=fG0iHHWGYXidSUTdCnIa0PgIsQSlpzvQ9yoKpuVRHsPqWQlF6np87g=="""
    
            let! response = client.PostAsync(uri, body) |> Async.AwaitTask
            response.EnsureSuccessStatusCode |> ignore
    
            let! htmlAsPdf =
                response.Content.ReadAsStreamAsync()
                |> Async.AwaitTask
            let stream = new MemoryStream()
            htmlAsPdf.CopyTo(stream)
            return stream.ToArray() |> Convert.ToBase64String 
    
        }

    let printEstimate (client:HttpClient)(estimate:EstimateDto) =
        estimateReport estimate
        |> print client
        |> Async.RunSynchronously