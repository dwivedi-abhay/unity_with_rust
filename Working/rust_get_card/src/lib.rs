use ludndev_hello_world::greet;
use std::ffi::{CString, CStr};
use std::os::raw::c_char;


#[no_mangle] pub extern "C" fn get_greetings(name : *const c_char) -> *mut c_char{


    let rust_string = unsafe {
        CStr::from_ptr(name).to_str().unwrap()
    };

    let greet_result = greet(rust_string);

    let c_string_result = CString :: new (greet_result).unwrap();

    c_string_result.into_raw()
}

#[no_mangle]
pub extern "C" fn free_string(ptr: *mut c_char) {
    
    if ptr.is_null() {
        return;
    }

   
    unsafe {
        CString::from_raw(ptr);
    }
} 

   

#[no_mangle] pub extern fn get_hello_world() -> i32{
     100
}
#[no_mangle] pub extern fn get_sum_of_two_numbers(a : &mut i32, b: &mut i32) -> i32{
    *a + *b
}
