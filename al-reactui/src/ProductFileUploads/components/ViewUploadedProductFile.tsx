import * as React from "react";
import { LookupTablesContextProvider } from "../../contexts/LookupTablesContext";
import { Form, Field, FormElement, FieldWrapper } from "@progress/kendo-react-form";
import { Grid, GridColumn, GridPageChangeEvent, GridFilterChangeEvent,GridSortChangeEvent } from '@progress/kendo-react-grid';
import useProductFileUpload from "../hooks/useProductFileUpload";
import {filterBy,CompositeFilterDescriptor,orderBy, SortDescriptor} from "@progress/kendo-data-query";
import { Button } from "@progress/kendo-react-buttons";
import VendorInputSheetUpload from "./VendorInputSheetUpload";
import ReactPlaceholder from "react-placeholder";
import {GridLayout, GridLayoutItem, TabStrip,TabStripSelectEventArguments,TabStripTab} from "@progress/kendo-react-layout";
import { Label } from "@progress/kendo-react-labels";
import { request } from "http";
import { REQUEST_STATUS } from "../../enums/enums";
import DetailHeader from "../../components/Shared/DetailHeader";


function ViewUploadedProductFile(props: any) { 

    //
    // Props 
    //
    const {FileID, onCloseCallBack} = props; 

    //
    // State/Context
    //
    const { dataViewProductFileUpload, ViewProductFileUpload } = useProductFileUpload(); // calls controller ViewUploadedProductFileData so we can see what a file contains

    // debugger; // displays data returned from hook

        // states for tracking diff tabs on the page 
    const [selectedTabIndex, setSelectedTabIndex] = React.useState<number>(0);
    const handleSelect = (e: TabStripSelectEventArguments) => {
        setSelectedTabIndex(e.selected);
    };

    //
    // Life Cycle
    //
    React.useEffect(() => { 
        debugger; 
        if (FileID > 0) {
            ViewProductFileUpload(FileID)
        }
    }, []
    )

    //
    // UI Events
    //


    //
    // Render
    //
    return (
        <div> 
            <ReactPlaceholder 
                ready={dataViewProductFileUpload !== undefined }>
                <DetailHeader
                    onCloseCallBack={onCloseCallBack}
                    title={"Viewing" + {} + "Contents"}
                    
                >
                </DetailHeader>

                <Grid
                    data={ dataViewProductFileUpload } 
                    pageable={true}
                >
                    <GridColumn field="supplierStyleName" title="Style Name" width="150px"/>
                    <GridColumn field="supplierStyleType" title="Style Type" width="150px"/>
                    <GridColumn field="productPrice_Old" title="Old Price" width="90px"/> 
                    <GridColumn field="productPrice_New" title="New Price" width="90px"/>
                    <GridColumn field="colorNames" title="Color Names"/>
                </Grid>

            </ReactPlaceholder>

            
        </div>

    );

}

export default ViewUploadedProductFile; 

/*
            <div>
                <GridLayout
                    gap={{ rows: 1, cols: 2 }}
                    cols={[{ width: "50%" }, { width: "50%" }]}
                >
                    <GridLayoutItem row={1} col={1} colSpan={1}>
                        <Label 
                            style = {{ 
                                alignItems: "end"
                            }}
                        >
                            {"hellooooo"}

                        </Label>
                    </GridLayoutItem>


                </GridLayout>
            </div>
 
*/