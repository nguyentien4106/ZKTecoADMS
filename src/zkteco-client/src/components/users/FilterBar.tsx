import { useEffect, useState } from "react";
import { zodResolver } from "@hookform/resolvers/zod";
import { useForm } from "react-hook-form";
import { z } from "zod";
import { MultiSelect } from "@/components/multi-select";
import { Button } from "@/components/ui/button";
import {
  Form,
  FormControl,
  FormField,
  FormItem,
  FormLabel,
  FormMessage,
} from "@/components/ui/form";
import { Device } from "@/types";

const FormSchema = z.object({
  deviceIds: z
    .array(z.string())
    .min(1, { message: "Please select at least one device." }),
});

interface FilterBarProps {
    devices: Device[] | undefined;
    handleSubmit?: (deviceIds: string[]) => void;
    selectedDeviceIds: string[]
}

export default function FilterBar({ devices, handleSubmit, selectedDeviceIds }: FilterBarProps) {
  const form = useForm<z.infer<typeof FormSchema>>({
    resolver: zodResolver(FormSchema),
    defaultValues: {
      deviceIds: selectedDeviceIds,
    },
  });

  function onSubmit(data: z.infer<typeof FormSchema>) {
    handleSubmit && handleSubmit(data.deviceIds);
  }

  useEffect(() => {
    form.reset({
      deviceIds: selectedDeviceIds,
    });
  }, [selectedDeviceIds]);
  
  return (
    <Form {...form}>
      <form onSubmit={form.handleSubmit(onSubmit)} className="flex items-center gap-4 mb-4 space-y-6">
        <FormField
          control={form.control}
          name="deviceIds"
          render={({ field }) => (
            <FormItem>
              <FormLabel aria-required>Devices * </FormLabel>
              <FormControl>
                <MultiSelect
                  options={devices ? devices.map(device => ({ value: device.id, label: device.deviceName })) : []}
                  value={field.value}
                  onValueChange={field.onChange}
                  placeholder="Choose devices..."
                  defaultValue={selectedDeviceIds}
                />
              </FormControl>
            <FormMessage />

            </FormItem>
          )}
          
        />

        <Button type="submit">Submit</Button>
      </form>
    </Form>
  );
}