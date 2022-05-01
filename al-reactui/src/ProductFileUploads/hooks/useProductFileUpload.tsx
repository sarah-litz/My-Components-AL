import {useState} from "react";
import { ProductUploadFilesViewModel } from "../../interfaces/interfaces"; 
import configData from "../../AppConfig.json";
import axios from "axios";
import { CardActions } from "@progress/kendo-react-layout";
export const REQUEST_STATUS = {
    LOADING: "Loading",
    SUCCESS: "success",
    FAILURE: "failure",
}


function useProductFileUpload(){ 
    const [dataProductFileUploadsList, setDataProductFileUploadsList] = useState<ProductUploadFilesViewModel[]>([]);
    const [dataViewProductFileUpload, setDataViewProductFileUpload] = useState<ProductUploadFilesViewModel[]>([]);
    const [requestStatus, setRequestStatus] = useState(REQUEST_STATUS.LOADING);

    //
    // Display for VendorInputSheetUpload.tsx
    //
    function ListProductFileUploads() { 
        async function ListProductFileUploadsAsynch() { 

            try { 
                const result = await axios.get(configData.SERVER_URL + "Product/ListUploadedProductFiles/"); 
                setRequestStatus(REQUEST_STATUS.SUCCESS); 
                setDataProductFileUploadsList(result.data); 
            }
            catch (e) { 
                setRequestStatus(REQUEST_STATUS.FAILURE); 
            }
        }
        ListProductFileUploadsAsynch(); 
    }



    //
    // Gets Data to display for ViewUploadedProductFile.tsx
    //
    function ViewProductFileUpload(fileID: number) { 

        async function ViewProductFileUploadAsynch() { 

            try { 
                
                debugger; 
                setRequestStatus(REQUEST_STATUS.LOADING); 
                const result = await axios.get(configData.SERVER_URL + "Product/ViewUploadedProductFileData/" + fileID.toString()); // {ProductFileUploadId}
                setDataViewProductFileUpload(result.data); 
                setRequestStatus(REQUEST_STATUS.SUCCESS); 

            } catch (e) { 
                
                setRequestStatus(REQUEST_STATUS.FAILURE)
            
            }
        }
        ViewProductFileUploadAsynch(); 

    }


    //
    // Invokes api call to get data from the ProductUploadData datatable and transfers/loads it into the actual Product datatable
    //
    function ProcessDataToProduct(fileID: number) { 

        async function ProcessDataToProductAsynch() { 
            
            try { 
                
                debugger; 
                setRequestStatus(REQUEST_STATUS.LOADING)
                const result = await axios.get(configData.SERVER_URL + "Product/ProcessDataToProduct/" + fileID.toString()); 
                // setRequestStatus(REQUEST_STATUS.SUCCESS)

            } catch (e) { 

                setRequestStatus(REQUEST_STATUS.FAILURE)

            }
            // api call 

        }
        ProcessDataToProductAsynch(); 

    }



    return {requestStatus,ListProductFileUploads,dataProductFileUploadsList,ViewProductFileUpload,dataViewProductFileUpload}
}
export default useProductFileUpload; 