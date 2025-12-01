// ==========================================
// src/components/dialogs/CreateUserDialog.tsx
// ==========================================
import { useEffect } from "react";
import { useForm } from "react-hook-form";
import { zodResolver } from "@hookform/resolvers/zod";
import { z } from "zod";
import {
  Dialog,
  DialogContent,
  DialogDescription,
  DialogHeader,
  DialogTitle,
} from "@/components/ui/dialog";
import { Input } from "@/components/ui/input";
import {
  Select,
  SelectContent,
  SelectGroup,
  SelectItem,
  SelectTrigger,
  SelectValue,
} from "@/components/ui/select";
import { UserPrivileges } from "@/constants";
import { defaultNewEmployee } from "@/constants/defaultValue";
import { Button } from "../ui/button";
import { useDevices } from "@/hooks/useDevices";
import { MultiSelect } from "../multi-select";
import {
  Form,
  FormField,
  FormItem,
  FormLabel,
  FormControl,
  FormMessage,
} from "@/components/ui/form";
import { PasswordInput } from "../password-input";
import { CreateEmployeeRequest, UpdateEmployeeRequest } from "@/types/employee";
import { useEmployeeContext } from "@/contexts/EmployeeContext";

const createFormSchema = z.object({
  pin: z.string().min(1, "PIN is required"),
  name: z.string().min(1, "Full name is required"),
  cardNumber: z.string().optional(),
  password: z.string().optional(),
  department: z.string().optional(),
  privilege: z.number(),
  deviceIds: z.array(z.string()).min(1, "Please select at least one device"),
});

const updateFormSchema = z.object({
  pin: z.string().min(1, "PIN is required"),
  name: z.string().min(1, "Full name is required"),
  cardNumber: z.string().optional(),
  password: z.string().optional(),
  department: z.string().optional(),
  privilege: z.number(),
  deviceIds: z.array(z.string()).optional(), // Not required for update
});

export const CreateUserDialog = () => {
  const {
    createDialogOpen,
    employeeToEdit,
    setCreateDialogOpen,
    handleAddEmployee,
    handleUpdateEmployee
  } = useEmployeeContext()
  const { data: devices } = useDevices();

  const FormSchema = employeeToEdit ? updateFormSchema : createFormSchema;

  const form = useForm<z.infer<typeof FormSchema>>({
    resolver: zodResolver(FormSchema),
    defaultValues: {
      ...defaultNewEmployee,
      deviceIds: [],
    },
  });

  useEffect(() => {
    if (employeeToEdit) {
      form.reset({
        pin: employeeToEdit.pin ?? '',
        name: employeeToEdit.name ?? '',
        cardNumber: employeeToEdit.cardNumber ?? '',
        password: employeeToEdit.password ?? '',
        department: employeeToEdit.department ?? '',
        privilege: employeeToEdit.privilege ?? 0,
        deviceIds: [],
      });
    } else {
      form.reset({
        ...defaultNewEmployee,
        deviceIds: [],
      });
    }
  }, [employeeToEdit, createDialogOpen]);

  const onSubmit = async (data: z.infer<typeof FormSchema>) => {
    if (employeeToEdit) {
      const updateParams = data as unknown as UpdateEmployeeRequest;
      updateParams.deviceId = employeeToEdit.deviceId;
      updateParams.userId = employeeToEdit.id ?? "";

      handleUpdateEmployee && (await handleUpdateEmployee(updateParams));
    } else {
      handleAddEmployee &&
        (await handleAddEmployee(data as unknown as CreateEmployeeRequest));
    }

    setCreateDialogOpen(false);
    form.reset({ ...defaultNewEmployee, deviceIds: [] });
  };

  return (
    <Dialog open={createDialogOpen} onOpenChange={setCreateDialogOpen}>
      <DialogContent className="sm:max-w-[600px] max-h-[90vh] overflow-y-auto">
        <DialogHeader>
          <DialogTitle>Add New User</DialogTitle>
          <DialogDescription>
            Create a new user and sync to devices
          </DialogDescription>
        </DialogHeader>
        <Form {...form}>
          <form onSubmit={form.handleSubmit(onSubmit)} className="space-y-4">
            <div className="grid grid-cols-1 sm:grid-cols-2 gap-4">
              <FormField
                control={form.control}
                name="deviceIds"
                render={({ field }) => (
                  <FormItem>
                    <FormLabel>Devices *</FormLabel>
                    <FormControl>
                      {employeeToEdit ? (
                        <Select disabled value={employeeToEdit.deviceId}>
                          <SelectTrigger>
                            <SelectValue>
                              {employeeToEdit.deviceName || "Select a device"}
                            </SelectValue>
                          </SelectTrigger>
                        </Select>
                      ) : (
                        <MultiSelect
                          options={
                            devices
                              ? devices.map((device) => ({
                                  value: device.id,
                                  label: device.deviceName,
                                }))
                              : []
                          }
                          value={Array.isArray(field.value) ? field.value : []}
                          onValueChange={field.onChange}
                          placeholder="Choose devices..."
                        />
                      )}
                    </FormControl>
                    <FormMessage className="absolute left-0 mt-1 text-destructive" />
                  </FormItem>
                )}
              />
              <FormField
                control={form.control}
                name="pin"
                render={({ field }) => (
                  <FormItem>
                    <FormLabel>PIN *</FormLabel>
                    <FormControl>
                      <Input
                        {...field}
                        placeholder="1001"
                        required
                        disabled={!!employeeToEdit}
                      />
                    </FormControl>
                    <FormMessage />
                  </FormItem>
                )}
              />
            </div>
            <div className="grid grid-cols-1 sm:grid-cols-2 gap-4">
              <FormField
                control={form.control}
                name="name"
                render={({ field }) => (
                  <FormItem>
                    <FormLabel>Name *</FormLabel>
                    <FormControl>
                      <Input {...field} placeholder="Nguyen Van A" required />
                    </FormControl>
                    <FormMessage />
                  </FormItem>
                )}
              />
              <FormField
                control={form.control}
                name="password"
                render={({ field }) => (
                  <FormItem>
                    <FormLabel>Password</FormLabel>
                    <FormControl>
                      <PasswordInput id="password" {...field} />
                    </FormControl>
                    <FormMessage className="absolute left-0 mt-1 text-destructive" />
                  </FormItem>
                )}
              />
            </div>

            <div className="grid grid-cols-1 sm:grid-cols-2 gap-4">
              <FormField
                control={form.control}
                name="cardNumber"
                render={({ field }) => (
                  <FormItem>
                    <FormLabel>Card Number</FormLabel>
                    <FormControl>
                      <Input {...field} placeholder="1234567890" />
                    </FormControl>
                    <FormMessage className="absolute left-0 mt-1 text-destructive" />
                  </FormItem>
                )}
              />
              <FormField
                control={form.control}
                name="privilege"
                render={({ field }) => (
                  <FormItem>
                    <FormLabel>Privilege</FormLabel>
                    <FormControl>
                      <Select
                        value={field.value?.toString()}
                        onValueChange={(value) => field.onChange(Number(value))}
                      >
                        <SelectTrigger>
                          <SelectValue placeholder="Select privilege" />
                        </SelectTrigger>
                        <SelectContent>
                          <SelectGroup>
                            {Object.keys(UserPrivileges).map((key) => (
                              <SelectItem key={key} value={key}>
                                {
                                  UserPrivileges[
                                    +key as keyof typeof UserPrivileges
                                  ]
                                }
                              </SelectItem>
                            ))}
                          </SelectGroup>
                        </SelectContent>
                      </Select>
                    </FormControl>
                    <FormMessage />
                  </FormItem>
                )}
              />
            </div>

            <div className="grid gap-4">
              <FormField
                control={form.control}
                name="department"
                render={({ field }) => (
                  <FormItem>
                    <FormLabel>Department</FormLabel>
                    <FormControl>
                      <Input {...field} placeholder="IT" />
                    </FormControl>
                    <FormMessage />
                  </FormItem>
                )}
              />
            </div>

            <div className="grid gap-4">
              <Button type="submit" className="ml-auto">
                { employeeToEdit ? "Update User" : "Create User" }
              </Button>
            </div>
          </form>
        </Form>
      </DialogContent>
    </Dialog>
  );
};
