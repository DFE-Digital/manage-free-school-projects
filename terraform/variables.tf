variable "environment" {
  description = "Environment name. Will be used along with `project_name` as a prefix for all resources."
  type        = string
}

variable "key_vault_access_users" {
  description = "List of users that require access to the Key Vault where tfvars are stored. This should be a list of User Principle Names (Found in Active Directory) that need to run terraform"
  type        = list(string)
}

variable "key_vault_access_ipv4" {
  description = "List of IPv4 Addresses that are permitted to access the Key Vault"
  type        = list(string)
}

variable "tfvars_filename" {
  description = "tfvars filename. This ensures that tfvars are kept up to date in Key Vault."
  type        = string
}

variable "project_name" {
  description = "Project name. Will be used along with `environment` as a prefix for all resources."
  type        = string
}

variable "azure_location" {
  description = "Azure location in which to launch resources."
  type        = string
}

variable "tags" {
  description = "Tags to be applied to all resources"
  type        = map(string)
}

variable "virtual_network_address_space" {
  description = "Virtual network address space CIDR"
  type        = string
}

variable "enable_container_registry" {
  description = "Set to true to create a container registry"
  type        = bool
}

variable "image_name" {
  description = "Image name"
  type        = string
}

variable "registry_admin_enabled" {
  description = "Do you want to enable access key based authentication for your Container Registry?"
  type        = bool
  default     = true
}

variable "registry_use_managed_identity" {
  description = "Create a User-Assigned Managed Identity for the Container App. Note: If you do not have 'Microsoft.Authorization/roleAssignments/write' permission, you will need to manually assign the 'AcrPull' Role to the identity"
  type        = bool
  default     = true
}

variable "registry_managed_identity_assign_role" {
  description = "Assign the 'AcrPull' Role to the Container App User-Assigned Managed Identity. Note: If you do not have 'Microsoft.Authorization/roleAssignments/write' permission, you will need to manually assign the 'AcrPull' Role to the identity"
  type        = bool
  default     = false
}

variable "container_command" {
  description = "Container command"
  type        = list(any)
}

variable "container_secret_environment_variables" {
  description = "Container secret environment variables"
  type        = map(string)
  sensitive   = true
}

variable "enable_mssql_database" {
  description = "Set to true to create an Azure SQL server/database, with a private endpoint within the virtual network"
  type        = bool
}

variable "mssql_server_admin_password" {
  description = "The local administrator password for the MSSQL server"
  type        = string
  default     = ""
  sensitive   = true
}

variable "mssql_azuread_admin_username" {
  description = "Username of a User within Azure AD that you want to assign as the SQL Server Administrator"
  type        = string
  default     = ""
}

variable "mssql_azuread_admin_object_id" {
  description = "Object ID of a User within Azure AD that you want to assign as the SQL Server Administrator"
  type        = string
  default     = ""
}

variable "mssql_database_name" {
  description = "The name of the MSSQL database to create. Must be set if `enable_mssql_database` is true"
  type        = string
  default     = ""
}

variable "mssql_firewall_ipv4_allow_list" {
  description = "A list of IPv4 Addresses that require remote access to the MSSQL Server"
  type        = list(string)
  default     = []
}

variable "mssql_server_public_access_enabled" {
  description = "Enable public internet access to your MSSQL instance. Be sure to specify 'mssql_firewall_ipv4_allow_list' to restrict inbound connections"
  type        = bool
  default     = false
}

variable "mssql_managed_identity_assign_role" {
  description = "Assign the 'Storage Blob Data Contributor' Role to the SQL Server User-Assigned Managed Identity. Note: If you do not have 'Microsoft.Authorization/roleAssignments/write' permission, you will need to manually assign the 'Storage Blob Data Contributor' Role to the identity"
  type        = bool
  default     = false
}

variable "enable_cdn_frontdoor" {
  description = "Enable Azure CDN FrontDoor. This will use the Container Apps endpoint as the origin."
  type        = bool
}

variable "cdn_frontdoor_origin_fqdn_override" {
  description = "Manually specify the hostname that the CDN Front Door should target. Defaults to the Container App FQDN"
  type        = string
  default     = ""
}

variable "cdn_frontdoor_origin_host_header_override" {
  description = "Manually specify the host header that the CDN sends to the target. Defaults to the recieved host header. Set to null to set it to the host_name (`cdn_frontdoor_origin_fqdn_override`)"
  type        = string
  default     = ""
  nullable    = true
}

variable "cdn_frontdoor_enable_rate_limiting" {
  description = "Enable CDN Front Door Rate Limiting. This will create a WAF policy, and CDN security policy. For pricing reasons, there will only be one WAF policy created."
  type        = bool
}

variable "cdn_frontdoor_rate_limiting_duration_in_minutes" {
  description = "CDN Front Door rate limiting duration in minutes"
  type        = number
}

variable "cdn_frontdoor_rate_limiting_threshold" {
  description = "CDN Front Door rate limiting duration in minutes"
  type        = number
}

variable "cdn_frontdoor_host_add_response_headers" {
  description = "List of response headers to add at the CDN Front Door `[{ \"name\" = \"Strict-Transport-Security\", \"value\" = \"max-age=31536000\" }]`"
  type        = list(map(string))
}

variable "enable_event_hub" {
  description = "Send Azure Container App logs to an Event Hub sink"
  type        = bool
  default     = false
}

variable "enable_logstash_consumer" {
  description = "Create an Event Hub consumer group for Logstash"
  type        = bool
  default     = false
}

variable "eventhub_export_log_analytics_table_names" {
  description = "List of Log Analytics table names that you want to export to Event Hub. See https://learn.microsoft.com/en-gb/azure/azure-monitor/logs/logs-data-export?tabs=portal#supported-tables for a list of supported tables"
  type        = list(string)
  default     = []
}

variable "enable_monitoring" {
  description = "Create an App Insights instance and notification group for the Container App"
  type        = bool
}

variable "monitor_email_receivers" {
  description = "A list of email addresses that should be notified by monitoring alerts"
  type        = list(string)
}

variable "existing_logic_app_workflow" {
  description = "Name, and Resource Group of an existing Logic App Workflow. Leave empty to create a new Resource"
  type = object({
    name : string
    resource_group_name : string
  })
  default = {
    name                = ""
    resource_group_name = ""
  }
}

variable "container_health_probe_path" {
  description = "Specifies the path that is used to determine the liveness of the Container"
  type        = string
}

variable "cdn_frontdoor_health_probe_path" {
  description = "Specifies the path relative to the origin that is used to determine the health of the origin."
  type        = string
}

variable "cdn_frontdoor_custom_domains" {
  description = "Azure CDN Front Door custom domains. If they are within the DNS zone (optionally created), the Validation TXT records and ALIAS/CNAME records will be created"
  type        = list(string)
}

variable "monitor_endpoint_healthcheck" {
  description = "Specify a route that should be monitored for a 200 OK status"
  type        = string
}

variable "existing_network_watcher_name" {
  description = "Use an existing network watcher to add flow logs."
  type        = string
}

variable "existing_network_watcher_resource_group_name" {
  description = "Existing network watcher resource group."
  type        = string
}

variable "container_apps_allow_ips_inbound" {
  description = "Restricts access to the Container Apps by creating a network security group rule that only allow inbound traffic from the provided list of IPs"
  type        = list(string)
  default     = []
}

variable "enable_dns_zone" {
  description = "Conditionally create a DNS zone"
  type        = bool
}

variable "cdn_frontdoor_forwarding_protocol" {
  description = "Azure CDN Front Door forwarding protocol"
  type        = string
  default     = "HttpsOnly"
}

variable "dns_zone_domain_name" {
  description = "DNS zone domain name. If created, records will automatically be created to point to the CDN."
  type        = string
}

variable "dns_ns_records" {
  description = "DNS NS records to add to the DNS Zone"
  type = map(
    object({
      ttl : optional(number, 300),
      records : list(string)
    })
  )
}

variable "dns_txt_records" {
  description = "DNS TXT records to add to the DNS Zone"
  type = map(
    object({
      ttl : optional(number, 300),
      records : list(string)
    })
  )
}

variable "enable_container_health_probe" {
  description = "Enable liveness probes for the Container"
  type        = bool
  default     = true
}

variable "cdn_frontdoor_health_probe_protocol" {
  description = "Use Http or Https"
  type        = string
  default     = "Https"
}

variable "custom_container_apps" {
  description = "Custom container apps, by default deployed within the container app environment managed by this module."
  type = map(object({
    container_app_environment_id = optional(string, "")
    resource_group_name          = optional(string, "")
    revision_mode                = optional(string, "Single")
    container_port               = optional(number, 0)
    ingress = optional(object({
      external_enabled = optional(bool, true)
      target_port      = optional(number, null)
      traffic_weight = object({
        percentage = optional(number, 100)
      })
      cdn_frontdoor_custom_domain = optional(string, "")
    }), null)
    secrets = optional(list(object({
      name  = string
      value = string
    })), [])
    registry = object({
      server               = optional(string, "")
      username             = optional(string, "")
      password_secret_name = optional(string, "")
      identity             = optional(string, "")
    }),
    image   = string
    cpu     = number
    memory  = number
    command = list(string)
    liveness_probes = optional(list(object({
      interval_seconds = number
      transport        = string
      port             = number
      path             = optional(string, null)
    })), [])
    env = optional(list(object({
      name      = string
      value     = optional(string, null)
      secretRef = optional(string, null)
    })), [])
    min_replicas = number
    max_replicas = number
  }))
  default = {}
}

variable "image_tag" {
  description = "Default image tag for the primary container"
  type        = string
  default     = "web-latest"
}

variable "statuscake_api_token" {
  description = "API token for StatusCake"
  type        = string
  sensitive   = true
  default     = "00000000000000000000000000000"
}

variable "statuscake_contact_group_name" {
  description = "Name of the contact group in StatusCake"
  type        = string
  default     = ""
}

variable "statuscake_contact_group_integrations" {
  description = "List of Integration IDs to connect to your Contact Group"
  type        = list(string)
  default     = []
}

variable "statuscake_monitored_resource_addresses" {
  description = "The URLs to perform TLS checks on"
  type        = list(string)
  default     = []
}

variable "statuscake_contact_group_email_addresses" {
  description = "List of email address that should receive notifications from StatusCake"
  type        = list(string)
  default     = []
}

variable "enable_container_app_blob_storage" {
  description = "Create an Azure Storage Account and Storage Container to be accessed by the Container App"
  type        = bool
}

variable "storage_account_ipv4_allow_list" {
  description = "A list of public IPv4 address to grant access to the Blob Storage Account"
  type        = list(string)
}

variable "storage_account_public_access_enabled" {
  description = "Should the Azure Storage Account have Public visibility?"
  type        = bool
}
