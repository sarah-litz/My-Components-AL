import React, {useEffect,useContext, useState} from "react";
import {LookupTablesContext} from "../../contexts/LookupTablesContext";
import { Form, Field, FormElement, FieldWrapper } from "@progress/kendo-react-form";
import { Label } from "@progress/kendo-react-labels";
import {DropDownList} from "@progress/kendo-react-dropdowns";
import { Dialog } from "@progress/kendo-react-dialogs";
import { Upload, UploadOnAddEvent, UploadOnProgressEvent, UploadFileInfo, } from "@progress/kendo-react-upload";
import configData from "../../AppConfig.json"; 






function VendorInputSheetUpload(props : any)
{
    //
    // Props // arguments that can be passed to components, allowing components to return React elements describing what should appear on the screen.
    //       // these are arguments passed in by the calling component, so ProductMaintenance 

    const {onCloseCallBack} = props;     
    
    //
    // State // similar to props, but it is private and fully controlled by a component
    //
    const {Manufacturers} = useContext(LookupTablesContext); 
    const [files, setFiles] = useState<Array<UploadFileInfo>>([]); 
    const [events, setEvents] = useState<Array<any>>([]);
    const [affectedFiles, setAffectedFiles] = useState<Array<UploadFileInfo>>([]);
    const [selectedManurfacturer, setSelectedManufacturer] = useState<number>(0); 

    //
    // Other Local Vars
    //
    const uploadUrl = configData.SERVER_URL + "Product/UploadVendorInputSheet/"  + selectedManurfacturer; 
    


    //
    // UI Events
    //
    const handleSubmit = () => { 
        debugger;
        onCloseCallBack()
    }
    const onAdd = (event: UploadOnAddEvent) => {
        debugger;
        setFiles(event.newState);
        setEvents([...events, `File selected: ${event.affectedFiles[0].name}`]);
        setAffectedFiles(event.affectedFiles);
    };
    const onProgress = (event: UploadOnProgressEvent) => {
        setFiles(event.newState);
        setEvents([...events, `On Progress: ${event.affectedFiles[0].progress} %`]);
    };  

    
    //
    // Render 
    //

    return (
        <Dialog 
            title = {'Vendor Input Sheet'}
            width = {'1000px'}
            onClose = {() => {onCloseCallBack()}}>
                <Form
                    onSubmit={handleSubmit}
                    render={(formRenderProps) => (
                        <FormElement
                            horizontal={true}
                            style={{maxWidth: 1000}}>
                            <FieldWrapper>
                                <Label 
                                    style={{
                                        alignItems: "end"
                                    }}
                                >
                                    {"Manufacturer"}
                                </Label>
                                <Field
                                    id={"manufacturerId"}
                                    name={"manufacturerId"}
                                    component={ DropDownList }
                                    data={  Manufacturers   }
                                    textField="manufacturerName"
                                    dataItemKey="manufacturerId"
                                    onChange={(event) => { setSelectedManufacturer(event.value.manufacturerId) }}
                                >
                                </Field>
                            </FieldWrapper>

                            {
                                <Upload
                                    autoUpload={true}
                                    batch={false}
                                    restrictions={{allowedExtensions: [".xlsx"]}}
                                    multiple={false}
                                    files={files}
                                    onAdd={onAdd}
                                    onProgress={onProgress}
                                    withCredentials={false}
                                    saveUrl={uploadUrl}
                                />
                            }

                            <div className="k-form-buttons">
                                <button
                                    type={"submit"}
                                    className="k-button"
                                    style={{width: "150px"}}
                                    disabled={false}
                                > 
                                    Save 
                                </button>
                                <button
                                    className="k-button"
                                    onClick={onCloseCallBack}
                                    style={{
                                        width: "150px",
                                    }}
                                > 
                                    Cancel
                                </button>                          
                            </div>
                        </FormElement>
                    )}                
                />
        </Dialog>        
    );
}
export default VendorInputSheetUpload;