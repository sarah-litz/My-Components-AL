
import {useState, useEffect} from "react";
import { LookupTablesContextProvider } from "../../contexts/LookupTablesContext";
import {Form,Field,FormElement,FormRenderProps} from "@progress/kendo-react-form";
import { Grid, GridColumn, GridPageChangeEvent, GridFilterChangeEvent,GridSortChangeEvent } from '@progress/kendo-react-grid';
import useProductFileUpload from "../hooks/useProductFileUpload";
import {filterBy,CompositeFilterDescriptor,orderBy, SortDescriptor} from "@progress/kendo-data-query";
import { Button } from "@progress/kendo-react-buttons";
import VendorInputSheetUpload from "./VendorInputSheetUpload";
import ReactPlaceholder from "react-placeholder";
import ViewUploadedProductFile from "./ViewUploadedProductFile"


interface PageState {
    skip: number;
    take: number;
  }

const initialDataState: PageState = { skip: 0, take: 1000 };

function ProductFileUploads(props: any) { 

    //
    // Props
    //
    




    //
    // State/Context 
    //
    const {dataProductFileUploadsList, ListProductFileUploads, ViewProductFileUpload} = useProductFileUpload(); 
    const initialFilter: CompositeFilterDescriptor = {
        logic: "and",
        filters: [],
    };
    const [filter, setFilter] = useState(initialFilter);
    const initialSort: Array<SortDescriptor> = [
        { field: "ProductName", dir: "asc" },
    ];
    const [sort, setSort] = useState(initialSort);
    const [page, setPage] = useState<PageState>(initialDataState);
    const pageChange = (event: GridPageChangeEvent) => {
        setPage(event.page);
      };
    const [OpenUploadVendorForm, setOpenUploadVendorForm] = useState<boolean>(false); 
    const [OpenViewProductFileUpload, setOpenViewProductFileUpload] = useState<boolean>(false); 
    const [selectedEntityId, setSelectedEntityId] = useState<number>(0);


    //
    // REACT Life Cycle (What happens when the component loads) 
    //
    useEffect( 
        // hook that executes when the component first loads 
        () => {
            ListProductFileUploads(); // List the Uploaded Product File Information
        }, 
        []
    )

    // 
    // UI Events
    //
    const handleUploadVendorClick = () => { 
        // debugger;
        setOpenUploadVendorForm(true); 
    }
    const handleCloseUploadVendorForm = () => { 
        // debugger;
        setOpenUploadVendorForm(false);
    }


    const handleCloseViewProductFileUpload = () => { 
        setOpenViewProductFileUpload(false); 
        // QUESTION -- should something else go here?? 
    }
    const handleViewButtonClick = (dataItem:any) => {
        // debugger;
        // opens the component ViewUPloadedProductFile.tsx
        setSelectedEntityId(dataItem.productFileUploadId)
        setOpenViewProductFileUpload(true); 
    }
    const handleDeleteButtonClick = () => {

        // TODO

    }
    const handleProcessToProductButtonClick = () => { 

        // TODO
        // open processtoproduct component
        // helpful example for opening diff "tabbed" component is the Versions.tsx's handleEditButtonClick
    }

    //
    // Local Components
    //
    
    // View Button: Causes chain of events to eventually open the ViewUploadedProductFile Component  
    const ViewButton = (props:any) => { 
        return ( 
            <td className="k-command-cell" > 
            <button
                className="k-button k-primary k-grid-save-command"
                onClick={
                    (event) => {
                        event.preventDefault();
                        handleViewButtonClick(props.dataItem);
                    }
                }
            >
                View
            </button>
        </td>      
        );
    }



    //
    // Render 
    //
    return ( 
        <ReactPlaceholder            
        ready={dataProductFileUploadsList !== undefined}>
            {

            }
            {
                !OpenViewProductFileUpload && 
                <div>             

                    <Form
                        initialValues={{}}
                        onSubmit={function () {return;}}
                        render={(formRenderProps : FormRenderProps) => (
                            <FormElement>
                                <table style={{ width: "60%" }}>
                                    <td  style={{ width: "20%", alignContent : "center" }}>
                                        <div className="k-form-buttons">
                                            <Button
                                            title="Add Product"
                                            className = "k-button k-primary"
                                            onClick={ handleUploadVendorClick }
                                            >
                                            Upload Vendor Input Sheet + 
                                            </Button>
                                        </div>
                                    </td>
                                </table>
                            </FormElement>
                        )}
                        >
                        </Form>
                        <Grid
                            data={
                                orderBy(
                                    filterBy(                                                                                            
                                        dataProductFileUploadsList?.slice(page.skip, page.take + page.skip),
                                        filter
                                    ),sort
                                )
                            }
                            pageable={true}
                            skip={page.skip}
                            take={page.take}    
                            total={dataProductFileUploadsList.length}    
                            onPageChange={pageChange}        
                            sortable={true}
                            sort={sort}
                            resizable={true}
                            filterable={true}
                            filter={filter}
                            onFilterChange={(e: GridFilterChangeEvent) => setFilter(e.filter)}
                            onSortChange={(e: GridSortChangeEvent) => {setSort(e.sort);}}
                        >
                            <GridColumn  
                                cell = {ViewButton} width="100px"
                            />
                            <GridColumn field="fileName" title="File Name" width="350px"    sortable={true} />
                            <GridColumn field="manufacturerName" title="Supplier" width="140px"    sortable={true} />
                            <GridColumn field="uploadedDate" title="Uploaded Date" width="120px" sortable={true} />
                            <GridColumn field="uploadedBy" title="Uploaded By" width="120px"    sortable={true} />
                            <GridColumn field="loadedDate" title="Loaded To Product Date" width="120px"    sortable={true} />
                            <GridColumn field="" title="# Errors" width="125px"    sortable={true} />
                            <GridColumn field="productFileUploadId" title="productFileUploadId" width="0px" />
                        </Grid>
                    </div>
                }
                
                
                {
                    OpenUploadVendorForm && 
                    <VendorInputSheetUpload 
                        onCloseCallBack = {handleCloseUploadVendorForm} >
                    </VendorInputSheetUpload>
                }
                {
                    OpenViewProductFileUpload && 
                    <ViewUploadedProductFile
                        FileID = {selectedEntityId}
                        onCloseCallBack = {handleCloseViewProductFileUpload}>
                    </ViewUploadedProductFile>
                }

        </ReactPlaceholder>           
    );
}

export default ProductFileUploads; 